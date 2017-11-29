using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UniversalTranspiler.Interfaces;
using UniversalTranspiler.Syntax;

namespace UniversalTranspiler
{
    public class DocumentParser : ILanguajeParser
    {
        private ParseableTokenStream TokenStream { get; set; }
        private Enums.Languaje _languaje;
        private LexerRepository _repository { get; set; }
        private Stack<JObject> _nodes;
        private JObject Currrent { get { return _nodes.Peek(); } }

        public DocumentParser(string source, Enums.Languaje lang)
        {
            _repository = new LexerRepository(lang);
            TokenStream = new ParseableTokenStream(new LexerTokenizer(source, lang));
            _languaje = lang;
            _nodes = new Stack<JObject>();
        }

        public object Parse()
        {
            var document = new JObject();
            _nodes.Push(document);
            ParseSyntax("Document", false);
            return Currrent;
        }

        private void ParseSyntax(string syntaxNodeStr, bool takeUntill)
        {
            var syntaxNode = _repository.GetSyntaxPattern(syntaxNodeStr);
            var syntaxReader = new SyntaxReader(syntaxNode, _languaje);
            var node = syntaxReader.Take(takeUntill);
            while (node != null)
            {
                var key = GetKeyFromNode(node);
                var parsedNode = Parse(node);
                if (parsedNode != null)
                    AddValueObject(Currrent,key,parsedNode);
                node = syntaxReader.Take();
            }
        }

        private void AddValueObject(JObject obj, string key, JToken value)
        {
            if(!_repository.IsKeyword(key) && !_repository.IsSpecialCharacter(key))
                obj[key] = value;
        }

        string GetKeyFromNode(ISyntaxNode node)
        {
            if (node is SyntaxNodeVariable noV)
                return noV.Name;
            else if (node is SyntaxSequence)
                return "values";
            else if (node is SyntaxNodeKeyword noK)
                return noK.Name;
            else if (node is SyntaxNodeGroup)
                return "val";
            return null;
        }
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
            return null;
        }
        private JArray Parse(SyntaxSequence seq)
        {
            JToken reminder = null;
            JArray result = null;
            do
            {
                var obj = new JObject();
                foreach (var node in seq.Nodes)
                {
                    var key = GetKeyFromNode(node);
                    reminder = Parse(node);
                    if (reminder != null)
                    {
                         AddValueObject(obj, key, reminder);
                    }
                }
                if (obj.HasValues)
                {
                    result = result ?? new JArray();
                    result.Add(obj);
                }
            } while (reminder != null);
            return result;
        }
        private JToken Parse(SyntaxNodeGroup seq)
        {
            foreach (var node in seq.Nodes)
            {
               var result = Parse(node);
                if (seq.IsOr && result != null)
                    return result;
                else
                {
                    if (result != null)
                    {
                        var key = GetKeyFromNode(node);
                        AddValueObject(Currrent, key, result);
                    }
                }
            }
            return null;
        }
        
        private JToken Parse(SyntaxNodeVariable node)
        {
            JObject syntaxObject = new JObject();
            _nodes.Push(syntaxObject);
            ParseSyntax(node.Name, node.TakeUntil);
            _nodes.Pop();
            if (syntaxObject.HasValues)
            {
                return syntaxObject;
            }
            else
                return null;

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
    }
    
}
