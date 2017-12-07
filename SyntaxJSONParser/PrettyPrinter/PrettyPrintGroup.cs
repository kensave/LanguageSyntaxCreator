using System.Collections.Generic;

namespace SyntaxJSONParser.PrettyPrinter
{
    internal class PrettyPrintGroup : IPrettyPrintNode
    {
        public bool Nullable { get; set ; }
        internal List<string> Keys { get; set; }
    }
}