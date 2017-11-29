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

            var sql = @"USE [InRS_TTAX]
                        GO
                        SET ANSI_NULLS ON
                        GO
                        SET QUOTED_IDENTIFIER OFF
                        GO
                        create PROCEDURE [dbo].[proc_Import_Tax_Rate_Chart]
                        	@Chart_ID char(16), @Jurisdiction_ID int, @Current_EOY_Rate float = 0, 
                            @Deferred_BOY_Rate float = 0, @Deferred_EOY_Rate float = 0
                        AS
                        BEGIN
                            if exists (select 'x' from Prov_Tax_Rate_Chart 
                                       where Prov_Chart_ID = @Chart_ID and Juris_Country_ID = @Jurisdiction_ID)
                            begin
                               update Prov_Tax_Rate_Chart 
                               set    Current_EOY_Rate = @Current_EOY_Rate, 
                                      Deferred_BOY_Rate = @Deferred_BOY_Rate, Deferred_EOY_Rate = @Deferred_EOY_Rate
                               where  Prov_Chart_ID = @Chart_ID and Juris_Country_ID = @Jurisdiction_ID
                            end
                            else
                            begin
                               insert into Prov_Tax_Rate_Chart (Prov_Chart_ID, Juris_Country_ID, Current_EOY_Rate, Deferred_BOY_Rate, Deferred_EOY_Rate)
                               values (@Chart_ID, @Jurisdiction_ID, @Current_EOY_Rate, @Deferred_BOY_Rate, @Deferred_EOY_Rate)
                            end
                        END
                        GO";
              parser = new DocumentParser(sql, Enums.Languaje.Sql);
              @ast = parser.Parse();

            var sps = Directory.GetFiles(@"C:\tr\poc\Screen Conversion\source\Extracted\StoredProcedures");
            var outDir = @"C:\tr\cust\migration_tools\output\Analisis\ASTs\StoreProcedures AST\";
            foreach (var sp in sps)
            {
                var spText = File.ReadAllText(sp);
                var p  = new DocumentParser(spText, Enums.Languaje.Sql);
                var json = p.Parse();
                File.WriteAllText(outDir + new FileInfo(sp).Name + ".json" , json.ToString());
            }
        }
    }
}
