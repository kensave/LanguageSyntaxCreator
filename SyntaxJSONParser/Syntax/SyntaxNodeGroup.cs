using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxJSONParser.Syntax
{
    public class SyntaxNodeGroup : ISyntaxNode
    {
        public bool Nullable { get; set; }
        public List<ISyntaxNode> Nodes { get; set; } = new List<ISyntaxNode>();
        public bool TakeUntil { get; set; }
        public bool IsOr { get; set; }
        public ISyntaxNode FoundNode { get; set; }
        public string Alias { get;set; }
    }
}
