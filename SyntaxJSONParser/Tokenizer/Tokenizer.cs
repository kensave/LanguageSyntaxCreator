using System;
using System.Globalization;
using System.Linq;

namespace SyntaxJSONParser
{
    internal class Tokenizer : TokenizableStreamBase<String>
    {
        public Tokenizer(String source)
            : base(() => source.ToCharArray().Select(i => i.ToString(CultureInfo.InvariantCulture)).ToList())
        {

        }
    }
}
