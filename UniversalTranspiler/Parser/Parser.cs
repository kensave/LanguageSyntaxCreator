using UniversalTranspiler.Enums;
using UniversalTranspiler.Interfaces;

namespace UniversalTranspiler
{
    public class Parser
    {
        private ILanguajeParser _parser;
        public Parser(Languaje lang, string code)
        {
            _parser = new LanguageParser(new LexerTokenizer(code, lang));
        }
    }
}
