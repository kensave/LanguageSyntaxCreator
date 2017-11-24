using System;


namespace UniversalTranspiler
{
    public interface IMatcher<T>
    {
        Token<T> IsMatch(Tokenizer tokenizer);
    }
}
