using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalTranspiler
{


    internal class ParseableTokenStream<T> : TokenizableStreamBase<Token<T>>
    {
        public ParseableTokenStream(Lexer<T> lexer) : base(() => lexer.Lex().ToList())
        {
        }
        public bool IsMatch(T type)
        {
            if (Equals(Current.TokenType, type))
            {
                return true;
            }

            return false;
        }
        public Token<T> Take(T type)
        {
            if (IsMatch(type))
            {
                var current = Current;

                Consume();

                return current;
            }

            throw new InvalidOperationException(String.Format("Invalid Syntax. Expecting {0} but got {1}", type, Current.TokenType));
        }

        public override Token<T> Peek(int lookahead)
        {
            var peeker = base.Peek(lookahead);

            if (peeker == null)
            {
                return new Token<T>((T) Enum.Parse(typeof(T), "EOF"));
            }

            return peeker;
        }

        public override Token<T> Current
        {
            get
            {
                var current = base.Current;
                if (current == null)
                {
                    return new Token<T>((T)Enum.Parse(typeof(T), "EOF"));
                }
                return current;
            }
        }
    }
}
