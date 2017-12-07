using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageSyntaxParser.PrettyPrinter
{
    public class DocumentPrettyPrinter
    {
        private LexerRepository _repository { get; set; }
        private Enums.Language _languaje;
        public DocumentPrettyPrinter(Enums.Language lang)
        {
            _repository = new LexerRepository(lang);
            _languaje = lang;
        }

        public string PrintNode(string key, string jsonNode)
        {
            var obj = JsonConvert.DeserializeObject(jsonNode) as JToken;
            return PrintNode(key, obj);
        }
            public string PrintNode(string key, JToken jsonNode)
        {
            //We reached a leaf
            string value = null;
            if (jsonNode is JValue val)
                value = val.Value<string>();
            StringBuilder result = new StringBuilder();
            //Get the specified Syntax from the Repository
            var prettyPrintNode = _repository.GetPrettyPrintPattern(key);
            if (prettyPrintNode == null && value != null)
                return value;
            if (prettyPrintNode == null)
                throw new KeyNotFoundException("Node " + key + " not supported for printing yet.");
            //Gets the first node.
            var prettyPrinteReader = new PrettyPrinterReader(prettyPrintNode, _repository);
            //Some nodes might have the takeUntil attribute.
            var node = prettyPrinteReader.Take();
            while (node != null)
            {
                //Tries to parse the Syntax.
                string printedNode = "";
                if (node is PrettyPrintVariable nv
                    && nv.Name == "Value"
                    && value != null)
                {
                    printedNode = value;
                }
                else
                {
                    printedNode = Print(node, jsonNode);
                }
                result.Append(printedNode);
                //Let's try to read the next token
                node = prettyPrinteReader.Take();
            }
            //Some nodes might have the takeUntil attribute.
            return result.ToString();
        }

        /// <summary>
        /// A parse method for every token type.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        private string Print(IPrettyPrintNode seq, JToken jsonNode)
        {
            if (seq is PrettyPrintVariable snv)
                return Print(snv, jsonNode);
            if (seq is PrettyPrintGroup ppg)
                return Print(ppg, jsonNode);
            else
            if (seq is PrettyPrintSequence ss)
                return Print(ss, jsonNode);
            else if (seq is PrettyPrintKeyword s)
                return Print(s, jsonNode);
            throw new NotSupportedException("Operation not supported");
        }
        private string Print(PrettyPrintSequence seq, JToken jsonNode)
        {
            StringBuilder result = new StringBuilder();
            var array = jsonNode as JArray;
            foreach (var node in array)
            {
                foreach (var nod in seq.Nodes)
                {
                    var nodeRes = Print(nod, node);
                    if (!nod.Nullable || (nod.Nullable && array.Last != node))
                    {
                        result.Append(nodeRes);
                    }
                }
            }
            return result.ToString();
        }

        
        private string Print(PrettyPrintGroup node, JToken jsonNode)
        {
            var jObject = jsonNode as JObject;
            foreach (var key in node.Keys)
            {
                string res = null;
                if (jsonNode[key] != null)
                {
                    res = PrintNode(key, jsonNode[key]);
                    if (res != null)
                        return res;
                }
            }
            return null;
        }
        private string Print(PrettyPrintKeyword node, JToken jsonNode)
        {
            var jObject = jsonNode as JObject;
            var res = jObject?[node.Name];
            if (res == null)
            {
                return _repository.TryGetValue(node.Name);
            }
            else
            {
                return res.Value<string>();
            }
        }

        private string Print(PrettyPrintVariable node,JToken jsonNode)
        {
            var nodVal = jsonNode[node.Name];
            if (nodVal == null)
                return null;
            return PrintNode(node.Name, jsonNode[node.Name]);
        }
        public string Print(JToken documentJson)
        {
            return PrintNode("Document", documentJson["Document"]);
        }
    }
}
