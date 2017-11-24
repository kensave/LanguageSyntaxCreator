using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalTranspiler
{
    internal class Token<T>
    {
        public T TokenType { get; private set; }

        public String TokenValue { get; private set; }

        public Token(T tokenType, String token)
        {
            TokenType = tokenType;
            TokenValue = token;
        }

        public Token(T tokenType)
        {
            TokenValue = null;
            TokenType = tokenType;
        }

        public override string ToString()
        {
            return TokenType + ": " + TokenValue;
        }
    }
}
