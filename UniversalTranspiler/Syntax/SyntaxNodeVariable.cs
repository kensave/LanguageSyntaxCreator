using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxJSONParser.Syntax
{
    public class SyntaxNodeVariable : ISyntaxNode
    {
        public bool Nullable { get; set; }
        public string Name { get; set; }
        public bool TakeUntil { get; set; }
    }
}
