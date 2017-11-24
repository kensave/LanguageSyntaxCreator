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
                            }
                ";
            var parser = new Parser(Enums.Languaje.CSharp, @class);
            var parser2 = new Parser(Enums.Languaje.Javascript, @class);
        }
    }
}
