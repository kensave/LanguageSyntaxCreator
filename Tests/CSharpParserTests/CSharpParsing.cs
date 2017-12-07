
namespace CSharpParserTests
{

    using SyntaxJSONParser;
    using SyntaxJSONParser.Enums;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using System;

    [TestFixture]
    public class CSharpParsing
    {
        [TestCase]
        public void BasicParsing()
        {
            var @class = 
                @" using System;
                            using System.Linq;
                            public class @Foo
                            {
                               
                            }
                ";
            var parser = new DocumentParser(Language.CSharp);
            var @ast = parser.Parse(@class) as JObject;
            Assert.IsNotNull(@ast);
            Assert.AreEqual(2,@ast["Document"]["Usings"].Children().Where(x => 
            {
                if (x.First is JProperty prop)
                {
                    return prop.Name == "UsingStatement";
                }
                else
                {
                    return false;
                }

            }).Count());
        }
    }
}
