using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SyntaxJSONParser
{
    internal class MatchWhiteSpace : MatcherBase
    {
        protected override Token IsMatchImpl(Tokenizer tokenizer, bool ignoreCase)
        {
            bool foundWhiteSpace = false;

            while (!tokenizer.End() && String.IsNullOrWhiteSpace(tokenizer.Current))
            {
                foundWhiteSpace = true;

                tokenizer.Consume();
            }

            if (foundWhiteSpace)
            {
                return new Token("WhiteSpace");
            }

            return null;
        }
    }
}
