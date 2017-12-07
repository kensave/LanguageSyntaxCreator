using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LanguageSyntaxParser
{
    internal class MatchKeyword : MatcherBase
    {
        public string Match { get; set; }

        private string TokenType { get; set; }


        /// <summary>
        /// If true then matching on { in a string like "{test" will match the first cahracter
        /// because it is not space delimited. If false it must be space or special character delimited
        /// </summary>
        public Boolean AllowAsSubString { get; set; }

        public List<MatchKeyword> SpecialCharacters { get; set; } 

        public MatchKeyword(string type, String match)
        {
            Match = match;
            TokenType = type;
            AllowAsSubString = true;
        }

        protected override Token IsMatchImpl(Tokenizer tokenizer, bool ignoreCase)
        {
            foreach (var character in Match)
            {
                var tokenChar = tokenizer.Current;
                var characterVal = character.ToString(CultureInfo.InvariantCulture);
                if (ignoreCase)
                {
                    tokenChar = tokenChar.ToLower();
                    characterVal = characterVal.ToLower();

                }
                if (tokenChar == characterVal)
                {
                    tokenizer.Consume();
                }
                else
                {
                    return null;
                }
            }

            bool found;

            if (!AllowAsSubString)
            {
                var next = tokenizer.Current;

                found = String.IsNullOrWhiteSpace(next) || SpecialCharacters.Any(character => character.Match == next);
            }
            else
            {
                found = true;
            }

            if (found)
            {
                return new Token(TokenType, Match);
            }

            return null;
        }
    }
}
