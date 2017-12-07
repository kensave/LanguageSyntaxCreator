namespace LanguageSyntaxParser
{
    /// <summary>
    /// A matcher is used to determine is something from the tokenizer is a match
    /// </summary>
    internal interface IMatcher
    {
        Token IsMatch(Tokenizer tokenizer, bool ignoreCase);
    }
}
