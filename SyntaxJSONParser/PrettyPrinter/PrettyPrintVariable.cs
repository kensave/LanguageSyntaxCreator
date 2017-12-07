namespace LanguageSyntaxParser.PrettyPrinter
{
    internal class PrettyPrintVariable : IPrettyPrintNode
    {
        public string Name { get; internal set; }
        public bool Nullable { get; set; }
    }
}