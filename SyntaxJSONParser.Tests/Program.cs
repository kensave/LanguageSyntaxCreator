using System.IO;

namespace SyntaxJSONParser.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var @class = @" using System;
                            public class @Foo
                            {
                                public string Smile()
                                  { return ""Smilie"" }
                                private double Once()
                                {
                                    return 1.1;
                                }
            
                            }
                ";
            var parser = new DocumentParser(@class, Enums.Languaje.CSharp);
            object @ast;// = parser.Parse();

            var sql = @"USE [Test]
                        GO
                        SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER OFF
                        GO
                        create PROCEDURE [dbo].[Test]
                        	@Test_ID char(16), @Other_ID int, @Current_Test_Rate float = 0, 
                            @Deferred_BOY_Rate float = 0, @Deferred_Test_Rate float = 0
                        GO";
            parser = new DocumentParser(sql, Enums.Languaje.Sql);
            @ast = parser.Parse();
        }
    }
}
