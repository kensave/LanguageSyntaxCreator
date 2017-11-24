using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniversalTranspiler.Enums;

namespace UniversalTranspiler
{
    internal class MatchingListFactory
    {
        private const string LANGUAGESSYNTAXDIR = "LanguagesSyntax";
        private static JObject jSonObject;

        public static List<IMatcher> GetMatchingList(Languaje lang)
        {
            List <IMatcher> list = new List<IMatcher>();
            var langKeyStr = lang.ToString();
            var file = Path.Combine(LANGUAGESSYNTAXDIR, langKeyStr + ".json");
            if (!File.Exists(file))
                throw new FileNotFoundException(file + " not found.");
            jSonObject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(file));


            var matchers = new List<IMatcher>(128);
            var keywordmatchers = GetKeywords();
            var specialCharacters = GetSpecialChars();
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
            matchers.AddRange(keywordmatchers);
            matchers.AddRange(specialCharacters);
            matchers.AddRange(new List<IMatcher>
                                                {
                                                    new MatchWhiteSpace(),
                                                    new MatchNumber(),
                                                    new MatchWord(specialCharacters)
                                                });
            return matchers;
        }

        private static List<IMatcher> GetSpecialChars()
        {
            List<IMatcher> result = new List<IMatcher>(64);
            var keywords = jSonObject["SpecialChars"] as JObject;
            foreach (var keyword in keywords)
                result.Add(new MatchKeyword(keyword.Key.ToString(), keyword.Value.ToString()));
            return result;
        }

        private static List<IMatcher> GetKeywords()
        {
            List<IMatcher> result = new List<IMatcher>(64);
            var keywords = jSonObject["Keywords"] as JObject;
            foreach (var keyword in keywords)
                result.Add(new MatchKeyword(keyword.Key.ToString(), keyword.Value.ToString()));
            return result;
        }
    }
}
