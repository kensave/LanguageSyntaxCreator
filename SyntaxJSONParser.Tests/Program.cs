using System.IO;
namespace LanguageSyntaxParser.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var @class = @" using System;
                            using System.Linq;
                            public class @Foo
                            {
                               
                            }
                ";
            var parser = new DocumentParser(Enums.Language.CSharp);
            object @ast = parser.Parse(@class).ToString();

            var sql = @"USE [Test]
                        GO
                        SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER OFF
                        GO
                        create PROCEDURE [dbo].[Test]
                        	@Test_ID char(16), @Other_ID int, @Current_Test_Rate float = 0, 
                            @Deferred_BOY_Rate numeric(15,0) = null, @Deferred_Test_Rate float = 0
                        GO";
              parser = new DocumentParser(Enums.Language.Sql);
              @ast = parser.Parse(sql).ToString();
        }
    }
}
