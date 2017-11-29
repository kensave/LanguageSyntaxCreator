using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxJSONParser.Syntax
{
    public interface ISyntaxNode
    {
        bool Nullable { get; set; }
        bool TakeUntil { get; set; }
    }
}
