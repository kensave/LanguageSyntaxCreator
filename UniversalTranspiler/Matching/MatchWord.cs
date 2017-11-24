using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalTranspiler
{
    public class MatchWord<T> : MatcherBase<T>
    {
        private List<MatchKeyword<T>> SpecialCharacters { get; set; } 
        public MatchWord(IEnumerable<IMatcher<T>> keywordMatchers)
        {
            SpecialCharacters = keywordMatchers.Select(i=>i as MatchKeyword<T>).Where(i=> i != null).ToList();
        }

        protected override Token<T> IsMatchImpl(Tokenizer tokenizer)
        {
            String current = null;

            while (!tokenizer.End() && !String.IsNullOrWhiteSpace(tokenizer.Current) && SpecialCharacters.All(m => m.Match != tokenizer.Current))
            {
                current += tokenizer.Current;
                tokenizer.Consume();
            }

            if (current == null)
            {
                return null;
            }

            // can't start a word with a special character
            if (SpecialCharacters.Any(c => current.StartsWith(c.Match)))
            {
                throw new InvalidOperationException(String.Format("Cannot start a word with a special character {0}", current));
            }

            return new Token<T>((T)Enum.Parse(typeof(T), "Word"), current);
        }
    }
}
