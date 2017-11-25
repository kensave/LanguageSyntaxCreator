using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTranspiler.Syntax
{
    public class SyntaxNodeOr : ISyntaxNode
    {
        public bool Nullable { get; set; }

        public List<ISyntaxNode> Nodes { get; set; } = new List<ISyntaxNode>();
    }
}
