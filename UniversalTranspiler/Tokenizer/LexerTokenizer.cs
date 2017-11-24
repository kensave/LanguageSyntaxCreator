using System;
using System.Collections.Generic;
using System.Linq;
using UniversalTranspiler.Enums;

namespace UniversalTranspiler
{
    internal class LexerTokenizer
    {
        private Tokenizer _tokenizer { get; set; }

        private IEnumerable<IMatcher> _matchers { get; set; }

        private Languaje _language;

        public LexerTokenizer(String source, Languaje lang)
        {
            _tokenizer = new Tokenizer(source);
            _language = lang;
        }

        internal IEnumerable<Token> Lex()
        {
            _matchers = MatchingListFactory.GetMatchingList(_language);
            var current = Next();

            while (current != null && !current.TokenType.Equals("EOF"))
            { 
                // skip whitespace
                if (!current.TokenType.Equals("WhiteSpace"))
                {
                    yield return current;
                }

                current = Next();
            }
        }
        private Token Next()
        {
            if (_tokenizer.End())
            {
                return new Token("EOF");
            }
            return
                 (from match in _matchers
                  let token = match.IsMatch(_tokenizer)
                  where token != null
                  select token).FirstOrDefault();
        }
    }
}
