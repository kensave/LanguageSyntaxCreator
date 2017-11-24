using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniversalTranspiler.Enums;

namespace UniversalTranspiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = new List<string>(args).Where(File.Exists);
            var str = files.Select(File.ReadAllText)
                           .Aggregate(string.Empty, (acc, item) => acc + Environment.NewLine + item);
            var t = new Parser(Languajes.CSharp, str);
        }
    }
}
