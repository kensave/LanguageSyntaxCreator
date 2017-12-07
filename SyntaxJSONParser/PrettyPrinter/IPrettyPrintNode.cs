using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxJSONParser.PrettyPrinter
{
    public interface IPrettyPrintNode
    {
        bool Nullable { get; set; }
    }
}
