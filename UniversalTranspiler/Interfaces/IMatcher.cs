using System;


namespace UniversalTranspiler
{
    internal interface IMatcher<T>
    {
        Token<T> IsMatch(Tokenizer tokenizer);
    }
}
