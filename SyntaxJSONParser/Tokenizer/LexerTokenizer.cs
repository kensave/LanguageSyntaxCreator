using System;
using System.Collections.Generic;
using System.Linq;
using SyntaxJSONParser.Enums;

namespace SyntaxJSONParser
{
    internal class LexerTokenizer
    {
        private Tokenizer _tokenizer { get; set; }

        private IEnumerable<IMatcher> _matchers { get; set; }

        private Languaje _language;

        private LexerRepository _repository;

        private bool _ignoreCase { get; set; }

        public LexerTokenizer(String source, Languaje lang)
        {
            _tokenizer = new Tokenizer(source);
            _language = lang;
            _repository = new LexerRepository(lang);
        }

        internal IEnumerable<Token> Lex()
        {
            _matchers = _repository.GetMatchingList();
            _ignoreCase = _repository.IsCaseIgnored();
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
                  let token = match.IsMatch(_tokenizer, _ignoreCase)
                  where token != null
                  select token).FirstOrDefault();
        }
    }
}
