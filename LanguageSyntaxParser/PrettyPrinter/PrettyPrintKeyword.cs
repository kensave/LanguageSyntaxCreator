namespace LanguageSyntaxParser.PrettyPrinter
{
    internal class PrettyPrintKeyword : IPrettyPrintNode
    {
        internal string Name { get; set; }
        public bool Nullable { get; set; }
    }
}