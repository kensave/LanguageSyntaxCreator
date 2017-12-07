using NUnit.Framework;
using LanguageSyntaxParser;
using LanguageSyntaxParser.Enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using LanguageSyntaxParser.PrettyPrinter;

namespace SQLServerTransactParserTests
{
    [TestFixture]
    public class SQLParsing
    {
        [TestCase]
        public void BasicParsingProcParams()
        {
            var @class =
                @"USE [Test]
                        GO
                        SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER OFF
                        GO
                        create PROCEDURE [dbo].[Test]
                        	@Test_ID char(16), @Other_ID int, @Current_Test_Rate float = 0, 
                            @Deferred_BOY_Rate numeric(15,0) = null, @Deferred_Test_Rate float = 0
                        GO";
            var parser = new DocumentParser(Language.Sql);
            var @ast = parser.Parse(@class) as JObject;
            NUnit.Framework.Assert.IsNotNull(@ast);
            NUnit.Framework.Assert.AreEqual(5, @ast["Document"]?[0]["CreateExpression"]?["Parameters"].Where(x =>
            {
                if (x.First is JProperty prop)
                {
                    return prop.Name == "Param";
                }
                else
                {
                    return false;
                }

            }).Count());
        }

        [TestCase]
        public void BasicParsingProcFullName()
        {
            var @class =
                @"USE [Test]
                        GO
                        SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER OFF
                        GO
                        create PROCEDURE [dbo].[Test]
                        	@Test_ID char(16), @Other_ID int, @Current_Test_Rate float = 0, 
                            @Deferred_BOY_Rate numeric(15,0) = null, @Deferred_Test_Rate float = 0
                        GO";
            var parser = new DocumentParser(Language.Sql);
            var @ast = parser.Parse(@class) as JObject;
            NUnit.Framework.Assert.IsNotNull(@ast);
            var identifier = @ast["Document"]?[0]["CreateExpression"]?["QualifiedIdentifier"];
            var prettyPrinter = new DocumentPrettyPrinter(Language.Sql);
            var name = prettyPrinter.PrintNode("QualifiedIdentifier", identifier);

            NUnit.Framework.Assert.AreEqual(name, "[dbo].[Test]");
        }

        [TestCase]
        public void BasicPrettyPrintJson()
        {
            var @class =
               @"USE [Test]
                        GO
                        SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER OFF
                        GO
                        create PROCEDURE [dbo].[Test]
                        	@Test_ID char(16), @Other_ID int, @Current_Test_Rate float = 0, 
                            @Deferred_BOY_Rate numeric(15,0) = null, @Deferred_Test_Rate float = 0
                        GO";
            var parser = new DocumentParser(Language.Sql);
            var @ast = parser.Parse(@class) as JObject;

            var prettyPrinter = new DocumentPrettyPrinter(Language.Sql);
            var identifierString = @ast["Document"]?[0]["CreateExpression"]?["QualifiedIdentifier"].ToString();
            var name = prettyPrinter.PrintNode("QualifiedIdentifier", identifierString);

            NUnit.Framework.Assert.AreEqual(name, "[dbo].[Test]");
        }

        [TestCase]
        public void BasicPrettyPrintProc()
        {
            var @class =
               @"USE [Test]
                        GO
                        SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER OFF
                        GO
                        create PROCEDURE [dbo].[Test]
                        	@Test_ID char(16), @Other_ID int, @Current_Test_Rate float = 0, 
                            @Deferred_BOY_Rate numeric(15,0) = null, @Deferred_Test_Rate float = 0
                        GO";
            var expected = @"CREATE PROCEDURE [dbo].[Test] @Test_ID char(16), @Other_ID int, @Current_Test_Rate float = 0, @Deferred_BOY_Rate numeric(15,0) = null, @Deferred_Test_Rate float = 0";
            var parser = new DocumentParser(Language.Sql);
            var @ast = parser.Parse(@class) as JObject;

            var prettyPrinter = new DocumentPrettyPrinter(Language.Sql);
            var doc = prettyPrinter.PrintNode("CreateExpression",@ast["Document"][0]["CreateExpression"]);

            NUnit.Framework.Assert.AreEqual(doc, expected);
        }
    }
}
