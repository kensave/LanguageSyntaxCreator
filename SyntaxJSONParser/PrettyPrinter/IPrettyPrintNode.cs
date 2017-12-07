using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageSyntaxParser.PrettyPrinter
{
    public interface IPrettyPrintNode
    {
        bool Nullable { get; set; }
    }
}
