using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTranspiler.Syntax
{
    public class SyntaxSequence : ISyntaxNode
    {
        public bool Nullable { get; set; }
        public List<ISyntaxNode> Nodes { get; set; } = new List<ISyntaxNode>();
        public bool TakeUntil { get; set; }
    }
}
