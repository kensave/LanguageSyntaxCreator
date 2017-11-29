using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxJSONParser
{
    internal class Token
    {
        public string TokenType { get; private set; }

        public String TokenValue { get; private set; }

        public Token(string tokenType, String token)
        {
            TokenType = tokenType;
            TokenValue = token;
        }

        public Token(string tokenType)
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
