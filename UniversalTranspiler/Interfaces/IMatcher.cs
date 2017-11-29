using System;


namespace UniversalTranspiler
{
    internal interface IMatcher
    {
        Token IsMatch(Tokenizer tokenizer, bool ignoreCase);
    }
}
