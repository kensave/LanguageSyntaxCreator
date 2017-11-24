using System;
using System.Globalization;
using System.Linq;

namespace UniversalTranspiler
{
    public class Tokenizer : TokenizableStreamBase<String>
    {
        public Tokenizer(String source)
            : base(() => source.ToCharArray().Select(i => i.ToString(CultureInfo.InvariantCulture)).ToList())
        {

        }
    }
}
