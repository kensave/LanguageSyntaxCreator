using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalTranspiler
{
    internal class MatchString<T> : MatcherBase<T>
    {
        public const string QUOTE = "\"";

        public const string TIC = "'";

        private String StringDelim { get; set; }

        public MatchString(String delim)
        {
            StringDelim = delim;
        }

        protected override Token<T> IsMatchImpl(Tokenizer tokenizer)
        {
            var str = new StringBuilder();

            if (tokenizer.Current == StringDelim)
            {
                tokenizer.Consume();

                while (!tokenizer.End() && tokenizer.Current != StringDelim)
                {
                    str.Append(tokenizer.Current);
                    tokenizer.Consume();
                }

                if (tokenizer.Current == StringDelim)
                {
                    tokenizer.Consume();
                }
            }

            if (str.Length > 0)
            {
                return new Token<T>((T)Enum.Parse(typeof(T), "QuotedString"), str.ToString());
            }

            return null;
        }
    }
}
