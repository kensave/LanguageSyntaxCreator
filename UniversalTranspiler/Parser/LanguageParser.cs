using UniversalTranspiler.Interfaces;

namespace UniversalTranspiler
{
    internal class LanguageParser<T> : ILanguajeParser
    {
        private ParseableTokenStream<T> TokenStream { get; set; }

        public LanguageParser(Lexer<T> lexer)
        {
            TokenStream = new ParseableTokenStream<T>(lexer);
        }
    }
    
}
