using System.Collections.Generic;

namespace LanguageSyntaxParser.PrettyPrinter
{
    internal class PrettyPrintSequence : IPrettyPrintNode
    {
        public bool Nullable { get; set ; }
        internal List<IPrettyPrintNode> Nodes { get; set; }
    }
}