using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalTranspiler
{
    internal class Lexer<T>
    {
        private Tokenizer Tokenizer { get; set; }

        private IEnumerable<IMatcher<T>> Matchers { get; set; }

        public Lexer(String source)
        {
            Tokenizer = new Tokenizer(source);
        }

        internal IEnumerable<Token<T>> Lex()
        {
            Matchers = MatchingListFactory.GetMatchingLIst<T>();

            var current = Next();

            while (current != null && !current.TokenType.Equals((T)Enum.Parse(typeof(T), "EOF")))
            {
                // skip whitespace
                if (!current.TokenType.Equals((T)Enum.Parse(typeof(T), "WhiteSpace")))
                {
                    yield return current;
                }

                current = Next();
            }
        }
        private Token<T> Next()
        {
            if (Tokenizer.End())
            {
                return new Token<T>((T)Enum.Parse(typeof(T), "EOF"));
            }

            return
                 (from match in Matchers
                  let token = match.IsMatch(Tokenizer)
                  where token != null
                  select token).FirstOrDefault();
        }
    }
}
