using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using LanguageSyntaxParser.Interfaces;
using LanguageSyntaxParser.Syntax;

namespace LanguageSyntaxParser
{
    /// <summary>
    /// A Language Document parser.
    /// </summary>
    public class DocumentParser : ILanguageParser
    {
        //Constants of Default identifiers of Sequences and Groups.
        private const string DEFAULTSEQUENCEID = "Values";
        private const string DEFAULTKEYWORDID = "Value";
        private ParseableTokenStream TokenStream { get; set; }
        private Enums.Language _languaje;
        private LexerRepository _repository { get; set; }
        /// <summary>
        /// Creates a new instance of the Document parser.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lang"></param>
        public DocumentParser(Enums.Language lang)
        {
            _repository = new LexerRepository(lang);
            _languaje = lang;
        }
        /// <summary>
        /// Parses a Document and returns an AST
        /// </summary>
        /// <returns></returns>
        public JObject Parse(string code)
        {
            //Resets the Token Stream.
            TokenStream = new ParseableTokenStream(new LexerTokenizer(code, _repository));

            var root = new JObject();
            var document = ParseSyntax("Document", false);
            root["Document"] = SimplifyObject(document);
            return root;
        }
        /// <summary>
        /// A parse method for every token type.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        private JToken Parse(ISyntaxNode seq)
        {
            if (seq is SyntaxNodeVariable snv)
                return Parse(snv);
            else if (seq is SyntaxSequence ss)
                return Parse(ss);
            else if (seq is SyntaxNodeKeyword s)
                return Parse(s);
            else if (seq is SyntaxNodeGroup sng)
                return Parse(sng);
            throw new NotSupportedException("Operation not supported");
        }
        /// <summary>
        /// Parses an Specific Language Syntax Previously Defined.
        /// </summary>
        /// <param name="syntaxNodeStr"></param>
        /// <param name="takeUntill"></param>
        /// <returns></returns>
        private JObject ParseSyntax(string syntaxNodeStr, bool takeUntill)
        {
            //Maybe we can get a partal match and we will need to revert.
            bool revertToSnapShot = false;
            try
            {
                //Let's take an Snapshot.
                TokenStream.TakeSnapshot();
                JObject result = null;
                //Get the specified Syntax from the Repository
                var syntaxNode = _repository.GetSyntaxPattern(syntaxNodeStr);
                //Gets the first node.
                var syntaxReader = new SyntaxReader(syntaxNode, _repository);
                //Some nodes might have the takeUntil attribute.
                var node = syntaxReader.Take(takeUntill);
                while (node != null)
                {
                    //Tries to parse the Syntax.
                    var parsedNode = Parse(node);
                    //It's allowed to be null only if it's nullable.
                    if (parsedNode == null && !node.Nullable)
                    {
                        //We need to go back in time...
                        revertToSnapShot = true;
                        return null;
                    }
                    //Something matched!!
                    if (parsedNode != null)
                        result = AddValueObject(result, node, parsedNode, syntaxNodeStr);
                    //Let's try to read the next token
                    node = syntaxReader.Take();
                }
                return result;
            }
            finally
            {
                //If a revert instruction was found. Let's go back.
                if (revertToSnapShot)
                {
                    TokenStream.RollbackSnapshot();
                }
                else
                {
                    TokenStream.CommitSnapshot();
                }
            }
        }
        private JArray Parse(SyntaxSequence seq)
        {
            JToken reminder = null;
            JArray result = null;
            do
            {
                JObject obj = null;
                foreach (var node in seq.Nodes)
                {
                    reminder = Parse(node);
                    if (reminder != null)
                    {
                        obj = AddValueObject(obj, node, reminder);
                    }
                }
                if (obj != null)
                {
                    result = result ?? new JArray();
                    result.Add(obj);
                }
            } while (reminder != null);
            return result;
        }
        private JToken Parse(SyntaxNodeGroup seq)
        {
            bool revertToSnapShot = false;
            JToken result = null;
            try
            {
                //Let's take an Snapshot.
                TokenStream.TakeSnapshot();
                foreach (var node in seq.Nodes)
                {
                    var res = Parse(node);
                    if (seq.IsOr && res != null)
                    {
                        seq.FoundNode = node;
                        result = res;
                        return result;
                    }
                    else if (!seq.IsOr)
                    {
                        //TODO: Rollback here
                        if (res == null && !node.Nullable)
                        {
                            revertToSnapShot = true;
                            return null;
                        }
                        if (res != null)
                        {
                            result = AddValueObject(result as JObject, node, res);
                        }
                    }
                }
            }
            finally
            {
                if (seq.IsOr && result == null && !seq.Nullable)
                    revertToSnapShot = true;
                if (revertToSnapShot)
                {
                    TokenStream.RollbackSnapshot();
                }
                else
                {
                    TokenStream.CommitSnapshot();
                }
            }
            return result;
        }

        private JToken Parse(SyntaxNodeVariable node)
        {
            return ParseSyntax(node.Name, node.TakeUntil);
        }
        private JToken Parse(SyntaxNodeKeyword node)
        {
            var nod = TokenStream.Take(node.Name, node.TakeUntil);
            if (nod != null)
            {
                return new JValue(nod.TokenValue);
            }
            return null;
        }
        /// <summary>
        /// Simplify the JObjects to reduce deep.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private JToken SimplifyObject(JToken value)
        {
            while (value is JObject && value[DEFAULTSEQUENCEID] != null && (value as JObject).Count == 1)
                value = value[DEFAULTSEQUENCEID];
            while (value is JObject && value[DEFAULTKEYWORDID] != null && (value as JObject).Count == 1)
                value = value[DEFAULTKEYWORDID];
            return value;
        }
        /// <summary>
        /// Adds a value to a JObject
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        private JObject AddValueObject(JObject obj, ISyntaxNode node, JToken value, string parentKey = null)
        {
            var key = GetKeyFromNode(node, parentKey);
            //If it's a Keyword of a SpecialChar let's ignore it to reduce the noise in the AST.
            if (!_repository.IsKeyword(key) && !_repository.IsSpecialCharacter(key))
            {
                //If null let's initialize tghe object.
                obj = obj ?? new JObject();
                var simplifiedVal = SimplifyObject(value);
                //If the Key is present. A merge will occur.
                if (obj[key] != null)
                {
                    if (obj[key] is JArray array)
                    {
                        array.Add(simplifiedVal);
                        obj[key] = simplifiedVal;
                    }
                    else
                    {
                        //TODO: Review object merging.
                        JArray values = new JArray
                        {
                            obj[key],
                            simplifiedVal
                        };
                        obj[key] = values;
                    }
                }
                else
                {
                    obj[key] = simplifiedVal;
                }
            }
            return obj;
        }
        /// <summary>
        /// Returns the Key for the result of the node parsing
        /// </summary>
        /// <param name="node"></param>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        private string GetKeyFromNode(ISyntaxNode node, string syntaxNode = null)
        {
            //If a variable or Keyword, lets return the Variable name it self.
            if (node is SyntaxNodeVariable noV)
                return noV.Name;
            else if (node is SyntaxNodeKeyword noK)
                return noK.Name;
            //If it's a sequence, lets use the alias or the default Key.
            else if (node is SyntaxSequence sq)
            {
                if (sq.Alias != null)
                    return sq.Alias;
                return DEFAULTSEQUENCEID;
            }
            //If it's a group the Key might be different depending if it's an Or group or not.
            else if (node is SyntaxNodeGroup sng)
            {
                if (!string.IsNullOrEmpty(sng.Alias))
                    return sng.Alias;
                string key = "";
                if (sng.IsOr)
                    key = GetKeyFromNode(sng.FoundNode);
                else
                    key = syntaxNode;
                if (_repository.IsKeyword(key))
                {
                    return DEFAULTKEYWORDID;
                }
                else
                    return key;
            }
            return null;
        }
       
    }

}
