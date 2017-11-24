using System;
using System.Collections.Generic;
using System.Linq;
using UniversalTranspiler.Enums;

namespace UniversalTranspiler
{
    public class MatchingListFactory
    {
        public static List<IMatcher<T>> GetMatchingLIst<T>()
        {
            var t = typeof(T);
            var list = new List<IMatcher<T>>();
            if (t == typeof(CSharpTokens))
            {
                foreach (var el in GetCSharpMatchingList())
                    list.Add((IMatcher<T>)el);
                return list;
            }
            else if (t == typeof(JSTokens))
            {
                foreach (var el in GetJSMatchingList())
                    list.Add((IMatcher<T>)el);
                return list;
            }
            return null;
        }
        public static List<IMatcher<JSTokens>> GetJSMatchingList()
        {
            var matchers = new List<IMatcher<JSTokens>>(64);

            var keywordmatchers = new List<IMatcher<JSTokens>>
                                  {
                                      new MatchKeyword<JSTokens>(JSTokens.Void, "void"),
                                      new MatchKeyword<JSTokens>(JSTokens.Int, "int"),
                                      new MatchKeyword<JSTokens>(JSTokens.Fun, "fun"),
                                      new MatchKeyword<JSTokens>(JSTokens.If, "if"),
                                      new MatchKeyword<JSTokens>(JSTokens.Infer, "var"),
                                      new MatchKeyword<JSTokens>(JSTokens.Else, "else"),
                                      new MatchKeyword<JSTokens>(JSTokens.While, "while"),
                                      new MatchKeyword<JSTokens>(JSTokens.For, "for"),
                                      new MatchKeyword<JSTokens>(JSTokens.Return, "return"),
                                      new MatchKeyword<JSTokens>(JSTokens.Print, "print"),
                                      new MatchKeyword<JSTokens>(JSTokens.True, "true"),
                                      new MatchKeyword<JSTokens>(JSTokens.False, "false"),
                                      new MatchKeyword<JSTokens>(JSTokens.Boolean, "bool"),
                                      new MatchKeyword<JSTokens>(JSTokens.New, "new"),
                                      new MatchKeyword<JSTokens>(JSTokens.Try, "try"),
                                      new MatchKeyword<JSTokens>(JSTokens.Catch, "catch")
                                  };


            var specialCharacters = new List<IMatcher<JSTokens>>
                                    {
                                        new MatchKeyword<JSTokens>(JSTokens.DeRef, "->"),
                                        new MatchKeyword<JSTokens>(JSTokens.LBracket, "{"),
                                        new MatchKeyword<JSTokens>(JSTokens.RBracket, "}"),
                                        new MatchKeyword<JSTokens>(JSTokens.LSquareBracket, "["),
                                        new MatchKeyword<JSTokens>(JSTokens.RSquareBracket, "]"),
                                        new MatchKeyword<JSTokens>(JSTokens.Plus, "+"),
                                        new MatchKeyword<JSTokens>(JSTokens.Minus, "-"),
                                        new MatchKeyword<JSTokens>(JSTokens.NotCompare, "!="),
                                        new MatchKeyword<JSTokens>(JSTokens.Compare, "=="),
                                        new MatchKeyword<JSTokens>(JSTokens.Equals, "="),
                                        new MatchKeyword<JSTokens>(JSTokens.HashTag, "#"),
                                        new MatchKeyword<JSTokens>(JSTokens.Comma, ","),
                                        new MatchKeyword<JSTokens>(JSTokens.OpenParenth, "("),
                                        new MatchKeyword<JSTokens>(JSTokens.CloseParenth, ")"),
                                        new MatchKeyword<JSTokens>(JSTokens.Asterix, "*"),
                                        new MatchKeyword<JSTokens>(JSTokens.Slash, "/"),
                                        new MatchKeyword<JSTokens>(JSTokens.Carat, "^"),
                                        new MatchKeyword<JSTokens>(JSTokens.Ampersand, "&"),
                                        new MatchKeyword<JSTokens>(JSTokens.GreaterThan, ">"),
                                        new MatchKeyword<JSTokens>(JSTokens.LessThan, "<"),
                                        new MatchKeyword<JSTokens>(JSTokens.Or, "||"),
                                        new MatchKeyword<JSTokens>(JSTokens.SemiColon, ";"),
                                        new MatchKeyword<JSTokens>(JSTokens.Dot, "."),
                                    };

            // give each keyword the list of possible delimiters and not allow them to be 
            // substrings of other words, i.e. token fun should not be found in string "function"
            keywordmatchers.ForEach(keyword =>
            {
                var current = (keyword as MatchKeyword<JSTokens>);
                current.AllowAsSubString = false;
                current.SpecialCharacters = specialCharacters.Select(i => i as MatchKeyword<JSTokens>).ToList();
            });

            matchers.Add(new MatchString<JSTokens>(MatchString<JSTokens>.QUOTE));
            matchers.Add(new MatchString<JSTokens>(MatchString<JSTokens>.TIC));
            matchers.AddRange(specialCharacters);
            matchers.AddRange(keywordmatchers);
            matchers.AddRange(new List<IMatcher<JSTokens>>
                                                {
                                                    new MatchWhiteSpace<JSTokens>(),
                                                    new MatchNumber<JSTokens>(),
                                                    new MatchWord<JSTokens>(specialCharacters)
                                                });

            return matchers;
        }
        public static List<IMatcher<CSharpTokens>> GetCSharpMatchingList()
        {
            var matchers = new List<IMatcher<CSharpTokens>>(64);

            var keywordmatchers = new List<IMatcher<CSharpTokens>>
                                  {
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Void, "void"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Int, "int"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Fun, "fun"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.If, "if"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Infer, "var"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Else, "else"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.While, "while"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.For, "for"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Return, "return"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Print, "print"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.True, "true"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.False, "false"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Boolean, "bool"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.String, "string"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Method, "method"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Class, "class"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.New, "new"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Nil, "nil"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Try, "try"),
                                      new MatchKeyword<CSharpTokens>(CSharpTokens.Catch, "catch")
                                  };


            var specialCharacters = new List<IMatcher<CSharpTokens>>
                                    {
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.DeRef, "->"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.LBracket, "{"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.RBracket, "}"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.LSquareBracket, "["),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.RSquareBracket, "]"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Plus, "+"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Minus, "-"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.NotCompare, "!="),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Compare, "=="),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Equals, "="),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.HashTag, "#"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Comma, ","),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.OpenParenth, "("),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.CloseParenth, ")"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Asterix, "*"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Slash, "/"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Carat, "^"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Ampersand, "&"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.GreaterThan, ">"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.LessThan, "<"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Or, "||"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.SemiColon, ";"),
                                        new MatchKeyword<CSharpTokens>(CSharpTokens.Dot, "."),
                                    };

            // give each keyword the list of possible delimiters and not allow them to be 
            // substrings of other words, i.e. token fun should not be found in string "function"
            keywordmatchers.ForEach(keyword =>
            {
                var current = (keyword as MatchKeyword<CSharpTokens>);
                current.AllowAsSubString = false;
                current.SpecialCharacters = specialCharacters.Select(i => i as MatchKeyword<CSharpTokens>).ToList();
            });

            matchers.Add(new MatchString<CSharpTokens>(MatchString<CSharpTokens>.QUOTE));
            matchers.Add(new MatchString<CSharpTokens>(MatchString<CSharpTokens>.TIC));
            matchers.AddRange(specialCharacters);
            matchers.AddRange(keywordmatchers);
            matchers.AddRange(new List<IMatcher<CSharpTokens>>
                                                {
                                                    new MatchWhiteSpace<CSharpTokens>(),
                                                    new MatchNumber<CSharpTokens>(),
                                                    new MatchWord<CSharpTokens>(specialCharacters)
                                                });

            return matchers;
        }
    }
}
