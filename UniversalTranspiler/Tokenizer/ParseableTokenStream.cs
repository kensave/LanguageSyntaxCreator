using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalTranspiler
{
    internal class ParseableTokenStream : TokenizableStreamBase<Token>
    {
        public ParseableTokenStream(LexerTokenizer lexer) : base(() => lexer.Lex().ToList())
        {
        }
        public bool IsMatch(string type)
        {
            if (Equals(Current.TokenType, type))
            {
                return true;
            }
            return false;
        }
        public Token Take(string type, bool takeUntil)
        {
            if (takeUntil)
                return TakeUntil(type);
            if (IsMatch(type))
            {
                var current = Current;

                Consume();

                return current;
            }
            return null;
            //throw new InvalidOperationException(String.Format("Invalid Syntax. Expecting {0} but got {1}", type, Current.TokenType));
        }
        public Token TakeUntil(string type)
        {
            Token current = null;
            while (!IsMatch(type))
            {
                current = Current;
                if (current.TokenType == "EOF")
                    return null;
                Consume();
            }
            current = Current;
            Consume();
            return current;
        }

        public override Token Peek(int lookahead)
        {
            var peeker = base.Peek(lookahead);

            if (peeker == null)
            {
                return new Token("EOF");
            }
            return peeker;
        }

        public override Token Current
        {
            get
            {
                var current = base.Current;
                if (current == null)
                {
                    return new Token("EOF");
                }
                return current;
            }
        }
    }
}
