using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageSyntaxParser.Enums;
using System.Reflection;

namespace LanguageSyntaxParser
{
    internal class LexerRepository
    {
        private const string LANGUAGESSYNTAXDIR = "LanguagesSyntax";
        private JObject jSonObject;
        private Dictionary<string, string> syntaxPatters = new Dictionary<string, string>(128);
        private Dictionary<string, string> prettyPrintPatters = new Dictionary<string, string>(128);
        private Dictionary<string, string> specialCharacters = new Dictionary<string, string>(128);
        private Dictionary<string, string> keywordmatchers = new Dictionary<string, string>(128);

        internal string GetPrettyPrintPattern(string key)
        {
            prettyPrintPatters.TryGetValue(key, out string res);
            return res;
        }

        private bool ignoreCase;
        private Language _language;
        private Dictionary<string, string> customkeywordmatchers = new Dictionary<string, string>(128);


        private string LoadDefinitionFromAssembly(string definitionName)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"LanguageSyntaxParser.{definitionName.Replace('\\', '.')}";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
            catch
            {
                return null;
            }

        }

        public LexerRepository(Language lang)
        {
            _language = lang;
            var langKeyStr = lang.ToString();

            var file = Path.Combine(LANGUAGESSYNTAXDIR, langKeyStr + ".json");
            string allText = "";
            if (!File.Exists(file))
            {
                allText = LoadDefinitionFromAssembly(file);
                if (allText == null)
                {
                    throw new FileNotFoundException(file + " not found and could not be found on the assembly resources either.");
                }
            }
            else
            {
                allText = File.ReadAllText(file);
            }
            jSonObject = (JObject)JsonConvert.DeserializeObject(allText);
            PopulateCollections();
        }

        internal string TryGetValue(string name)
        {
            var result = "";
            if (specialCharacters.TryGetValue(name, out result))
                return result;
            if (keywordmatchers.TryGetValue(name, out result))
                return result;
            if (customkeywordmatchers.TryGetValue(name, out result))
                return result;
            return result;
        }

        public bool IsKeyword(string key)
        {
            return keywordmatchers.ContainsKey(key);
        }
        public bool IsSpecialCharacter(string key)
        {
            return specialCharacters.ContainsKey(key);
        }
        public bool IsSyntaxPattern(string key)
        {
            return syntaxPatters.ContainsKey(key);
        }
        public string GetSyntaxPattern(string key)
        {
            return syntaxPatters[key];
        }

        public bool IsCaseIgnored()
        {
            return ignoreCase;

        }
        public List<IMatcher> GetMatchingList()
        {
            List<IMatcher> list = new List<IMatcher>();

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

        internal bool IsCustomKeyword(string name)
        {
            return customkeywordmatchers.ContainsKey(name);
        }

        private List<IMatcher> GetSpecialChars()
        {
            List<IMatcher> result = new List<IMatcher>(64);
            foreach (var special in specialCharacters)
            {
                result.Add(new MatchKeyword(special.Key, special.Value));
            }
            return result;
        }

        private List<IMatcher> GetKeywords()
        {
            List<IMatcher> result = new List<IMatcher>(64);
            foreach (var keyword in keywordmatchers)
            {
                result.Add(new MatchKeyword(keyword.Key, keyword.Value));
            }
            return result;
        }

        private List<IMatcher> GetSyntaxPatterns()
        {
            List<IMatcher> result = new List<IMatcher>(64);
            foreach (var syntaxPatt in syntaxPatters)
            {
                result.Add(new MatchKeyword(syntaxPatt.Key, syntaxPatt.Value));
            }
            return result;
        }

        private void PopulateCollections()
        {
            var keywords = jSonObject["Keywords"] as JObject;
            foreach (var keyword in keywords)
            {
                keywordmatchers.Add(keyword.Key.ToString(), keyword.Value.ToString());
            }
            var custom = jSonObject["CustomKeywords"] as JObject;
            foreach (var keyword in custom)
            {
                customkeywordmatchers.Add(keyword.Key.ToString(), keyword.Value.ToString());
            }
            var syntaxPatts = jSonObject["Syntax"] as JObject;
            foreach (var syntaxPatt in syntaxPatts)
            {
                syntaxPatters.Add(syntaxPatt.Key.ToString(), syntaxPatt.Value.ToString());
            }

            var prettyPatts = jSonObject["PrettyPrint"] as JObject;
            foreach (var syntaxPatt in prettyPatts)
            {
                prettyPrintPatters.Add(syntaxPatt.Key.ToString(), syntaxPatt.Value.ToString());
            }
            var specialChar = jSonObject["SpecialChars"] as JObject;
            foreach (var special in specialChar)
            {
                specialCharacters.Add(special.Key.ToString(), special.Value.ToString());
            }
            var ignoreCaseValue = jSonObject["IgnoreCase"];
            if (ignoreCaseValue != null)
                ignoreCase = Convert.ToBoolean(ignoreCaseValue);

        }
    }
}
