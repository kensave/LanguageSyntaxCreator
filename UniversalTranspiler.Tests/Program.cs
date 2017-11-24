using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var parser = new Parser(Enums.Languajes.CSharp, @class);
            var parser2 = new Parser(Enums.Languajes.Javascript, @class);
        }
    }
}
