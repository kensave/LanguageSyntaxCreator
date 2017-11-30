using System;
using System.Collections.Generic;
using System.Linq;

namespace SyntaxJSONParser
{
    internal class MatchWord : MatcherBase
    {
        private List<MatchKeyword> SpecialCharacters { get; set; } 
        public MatchWord(IEnumerable<IMatcher> keywordMatchers)
        {
            SpecialCharacters = keywordMatchers.Select(i=>i as MatchKeyword).Where(i=> i != null).ToList();
        }

        protected override Token IsMatchImpl(Tokenizer tokenizer, bool ignoreCase)
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

            return new Token("Identifier", current);
        }
    }
}
