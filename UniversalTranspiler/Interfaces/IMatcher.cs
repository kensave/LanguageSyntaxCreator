using System;


namespace SyntaxJSONParser
{
    internal interface IMatcher
    {
        Token IsMatch(Tokenizer tokenizer, bool ignoreCase);
    }
}
