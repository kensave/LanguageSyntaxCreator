using UniversalTranspiler.Interfaces;

namespace UniversalTranspiler
{
    internal class LanguageParser : ILanguajeParser
    {
        private ParseableTokenStream TokenStream { get; set; }

        public LanguageParser(LexerTokenizer lexer)
        {
            TokenStream = new ParseableTokenStream(lexer);
        }
    }
    
}
