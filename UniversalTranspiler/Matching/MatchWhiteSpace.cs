using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace UniversalTranspiler
{
    class MatchWhiteSpace<T> : MatcherBase<T>
    {
        protected override Token<T> IsMatchImpl(Tokenizer tokenizer)
        {
            bool foundWhiteSpace = false;

            while (!tokenizer.End() && String.IsNullOrWhiteSpace(tokenizer.Current))
            {
                foundWhiteSpace = true;

                tokenizer.Consume();
            }

            if (foundWhiteSpace)
            {
                return new Token<T>((T)Enum.Parse(typeof(T), "WhiteSpace"));
            }

            return null;
        }
    }
}
