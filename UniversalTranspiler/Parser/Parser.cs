using UniversalTranspiler.Enums;
using UniversalTranspiler.Interfaces;

namespace UniversalTranspiler
{
    public class Parser
    {
        private ILanguajeParser _parser;
        public Parser(Languajes lang, string code)
        {
            switch (lang)
            {
                case Languajes.CSharp:
                    _parser = new LanguageParser<CSharpTokens>(new Lexer<CSharpTokens>(code));
                        break;
                case Languajes.Javascript:
                    _parser = new LanguageParser<JSTokens>(new Lexer<JSTokens>(code));
                    break;

            }
        }
    }
}
