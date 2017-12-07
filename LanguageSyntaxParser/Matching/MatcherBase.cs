
using System;
namespace LanguageSyntaxParser
{
    internal abstract class MatcherBase : IMatcher
    {
        public Token IsMatch(Tokenizer tokenizer, bool ignoreCase)
        {
            if (tokenizer.End())
            {
                return new Token("EOF", "EOF");
            }

            tokenizer.TakeSnapshot();

            var match = IsMatchImpl(tokenizer, ignoreCase);

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

        protected abstract Token IsMatchImpl(Tokenizer tokenizer, bool ignoreCase);
    }
}
