namespace UniversalTranspiler.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var @class = @" using System;
                            public class @Foo()
                            {
                                public string Smile()
                                  { return ""Smilie"" }
                                private double Once()
                                {
                                    return 1.1;
                                }
            
                            }
                ";
            var parser = new DocumentParser(@class, Enums.Languaje.CSharp );
            var @ast = parser.Parse();
        }
    }
}
