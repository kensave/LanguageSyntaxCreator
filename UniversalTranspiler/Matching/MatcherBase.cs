
using System;
namespace UniversalTranspiler
{
    internal abstract class MatcherBase<T> : IMatcher<T>
    {
        public Token<T> IsMatch(Tokenizer tokenizer)
        {
            if (tokenizer.End())
            {
                return new Token<T>((T)Enum.Parse(typeof(T), "EOF"));
            }

            tokenizer.TakeSnapshot();

            var match = IsMatchImpl(tokenizer);

            if (match == null)
            {
                tokenizer.RollbackSnapshot();
            }
            else
            {
                tokenizer.CommitSnapshot();
            }

            return match;
        }

        protected abstract Token<T> IsMatchImpl(Tokenizer tokenizer);
    }
}
