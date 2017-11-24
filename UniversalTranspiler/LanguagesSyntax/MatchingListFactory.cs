using System;
using System.Collections.Generic;
using System.Linq;
using UniversalTranspiler.Enums;

namespace UniversalTranspiler
{
    internal class MatchingListFactory
    {
        public static List<IMatcher> GetMatchingList(Languaje lang)
        {
            List <IMatcher> list = new List<IMatcher>();
            if (lang == Languaje.CSharp)
            {
                return GetCSharpMatchingList();
            }
            else if (lang == Languaje.Javascript)
            {
                return GetJSMatchingList();
            }
            return null;
        }
        public static List<IMatcher> GetJSMatchingList()
        {
            var matchers = new List<IMatcher>(64);

            var keywordmatchers = new List<IMatcher>
                                  {
                                      new MatchKeyword("Void", "void"),
                                      new MatchKeyword("Int", "int"),
                                      new MatchKeyword("If", "if"),
                                      new MatchKeyword("Infer", "var"),
                                      new MatchKeyword("Else", "else"),
                                      new MatchKeyword("While", "while"),
                                      new MatchKeyword("For", "for"),
                                      new MatchKeyword("Return", "return"),
                                      new MatchKeyword("Print", "print"),
                                      new MatchKeyword("True", "true"),
                                      new MatchKeyword("False", "false"),
                                      new MatchKeyword("Boolean", "bool"),
                                      new MatchKeyword("New", "new"),
                                      new MatchKeyword("Try", "try"),
                                      new MatchKeyword("Catch", "catch")
                                  };


            var specialCharacters = new List<IMatcher>
                                    {
                                        new MatchKeyword("LBracket", "{"),
                                        new MatchKeyword("RBracket", "}"),
                                        new MatchKeyword("LSquareBracket", "["),
                                        new MatchKeyword("RSquareBracket", "]"),
                                        new MatchKeyword("Plus", "+"),
                                        new MatchKeyword("Minus", "-"),
                                        new MatchKeyword("NotCompare", "!="),
                                        new MatchKeyword("Compare", "=="),
                                        new MatchKeyword("Equals", "="),
                                        new MatchKeyword("HashTag", "#"),
                                        new MatchKeyword("Comma", ","),
                                        new MatchKeyword("OpenParenth", "("),
                                        new MatchKeyword("CloseParenth", ")"),
                                        new MatchKeyword("Asterix", "*"),
                                        new MatchKeyword("Slash", "/"),
                                        new MatchKeyword("Carat", "^"),
                                        new MatchKeyword("Ampersand", "&"),
                                        new MatchKeyword("GreaterThan", ">"),
                                        new MatchKeyword("LessThan", "<"),
                                        new MatchKeyword("Or", "||"),
                                        new MatchKeyword("SemiColon", ";"),
                                        new MatchKeyword("Dot", "."),
                                    };

            // give each keyword the list of possible delimiters and not allow them to be 
            // substrings of other words, i.e. token fun should not be found in string "function"
            keywordmatchers.ForEach(keyword =>
            {
                var current = (keyword as MatchKeyword);
                current.AllowAsSubString = false;
                current.SpecialCharacters = specialCharacters.Select(i => i as MatchKeyword).ToList();
            });

            matchers.Add(new MatchString(MatchString.QUOTE));
            matchers.Add(new MatchString(MatchString.TIC));
            matchers.AddRange(specialCharacters);
            matchers.AddRange(keywordmatchers);
            matchers.AddRange(new List<IMatcher>
                                                {
                                                    new MatchWhiteSpace(),
                                                    new MatchNumber(),
                                                    new MatchWord(specialCharacters)
                                                });

            return matchers;
        }
        public static List<IMatcher> GetCSharpMatchingList()
        {
            var matchers = new List<IMatcher>(64);

            var keywordmatchers = new List<IMatcher>
                                  {
                                      new MatchKeyword("Void", "void"),
                                      new MatchKeyword("Int", "int"),
                                      new MatchKeyword("Fun", "fun"),
                                      new MatchKeyword("If", "if"),
                                      new MatchKeyword("Infer", "var"),
                                      new MatchKeyword("Else", "else"),
                                      new MatchKeyword("While", "while"),
                                      new MatchKeyword("For", "for"),
                                      new MatchKeyword("Return", "return"),
                                      new MatchKeyword("Print", "print"),
                                      new MatchKeyword("True", "true"),
                                      new MatchKeyword("False", "false"),
                                      new MatchKeyword("Boolean", "bool"),
                                      new MatchKeyword("String", "string"),
                                      new MatchKeyword("Method", "method"),
                                      new MatchKeyword("Class", "class"),
                                      new MatchKeyword("Using", "using"),
                                      new MatchKeyword("New", "new"),
                                      new MatchKeyword("Nil", "nil"),
                                      new MatchKeyword("Try", "try"),
                                      new MatchKeyword("Catch", "catch")
                                  };


            var specialCharacters = new List<IMatcher>
                                    {
                                        new MatchKeyword("DeRef", "->"),
                                        new MatchKeyword("LBracket", "{"),
                                        new MatchKeyword("RBracket", "}"),
                                        new MatchKeyword("LSquareBracket", "["),
                                        new MatchKeyword("RSquareBracket", "]"),
                                        new MatchKeyword("Plus", "+"),
                                        new MatchKeyword("Minus", "-"),
                                        new MatchKeyword("NotCompare", "!="),
                                        new MatchKeyword("Compare", "=="),
                                        new MatchKeyword("Equals", "="),
                                        new MatchKeyword("HashTag", "#"),
                                        new MatchKeyword("Comma", ","),
                                        new MatchKeyword("OpenParenth", "("),
                                        new MatchKeyword("CloseParenth", ")"),
                                        new MatchKeyword("Asterix", "*"),
                                        new MatchKeyword("Slash", "/"),
                                        new MatchKeyword("Carat", "^"),
                                        new MatchKeyword("Ampersand", "&"),
                                        new MatchKeyword("GreaterThan", ">"),
                                        new MatchKeyword("LessThan", "<"),
                                        new MatchKeyword("Or", "||"),
                                        new MatchKeyword("SemiColon", ";"),
                                        new MatchKeyword("Dot", "."),
                                    };

            // give each keyword the list of possible delimiters and not allow them to be 
            // substrings of other words, i.e. token fun should not be found in string "function"
            keywordmatchers.ForEach(keyword =>
            {
                var current = (keyword as MatchKeyword);
                current.AllowAsSubString = false;
                current.SpecialCharacters = specialCharacters.Select(i => i as MatchKeyword).ToList();
            });

            matchers.Add(new MatchString(MatchString.QUOTE));
            matchers.Add(new MatchString(MatchString.TIC));
            matchers.AddRange(specialCharacters);
            matchers.AddRange(keywordmatchers);
            matchers.AddRange(new List<IMatcher>
                                                {
                                                    new MatchWhiteSpace(),
                                                    new MatchNumber(),
                                                    new MatchWord(specialCharacters)
                                                });

            return matchers;
        }
    }
}
