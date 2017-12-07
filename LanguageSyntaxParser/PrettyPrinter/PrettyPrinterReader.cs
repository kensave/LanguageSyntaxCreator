using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageSyntaxParser.PrettyPrinter
{
    internal class PrettyPrinterReader
    {
        private int Index { get; set; }
        private string _syntaxPattern;
        private LexerRepository _repository;
        internal PrettyPrinterReader(string prettyPrintPattern, LexerRepository repository)
        {
            _syntaxPattern = prettyPrintPattern.Replace(" ", "");
            _repository = repository;
        }

        internal IPrettyPrintNode Take()
        {
            if (Index == _syntaxPattern.Length)
                return null;
            char node = _syntaxPattern[Index];
            Index++;
            switch (node)
            {
                case '{':
                    return TakePlaceholder();
                case '<':
                    return TakeSequence();
                case '(':
                    return TakeGroup();
            }
            return null;
        }

        private IPrettyPrintNode TakeGroup()
        {
            var nodes = new List<string>();
            while (_syntaxPattern[Index] != ')')
            {
                var taken = Take();
                if (taken is PrettyPrintKeyword ppk)
                    nodes.Add(ppk.Name);
                else if (taken is PrettyPrintVariable ppv)
                    nodes.Add(ppv.Name);
                if (_syntaxPattern[Index] == '|')
                    Index++;
            }
            return new PrettyPrintGroup()
            {
                Keys = nodes
            };

        }

        private IPrettyPrintNode TakeSequence()
        {
            var nodes = new List<IPrettyPrintNode>();
            while (_syntaxPattern[Index] != '>')
            {
                nodes.Add(Take());
                if (_syntaxPattern[Index] == ',')
                    Index++;
            }
            Index++;
            var seq = new PrettyPrintSequence()
            {
                Nodes = nodes,
            };
            return seq;
        }

        private IPrettyPrintNode TakePlaceholder()
        {
            var name = "";
            while (_syntaxPattern[Index] != '}')
            {
                name += _syntaxPattern[Index];
                Index++;
            }
            Index++;
            bool nullable = IsNullable();
            if (_repository.IsSpecialCharacter(name) || _repository.IsKeyword(name) || _repository.IsCustomKeyword(name))
            {
                return new PrettyPrintKeyword()
                {
                    Name = name,
                    Nullable = nullable,
                };
            }
            else
            {
                return new PrettyPrintVariable()
                {
                    Name = name,
                    Nullable = nullable,
                };
            }

        }

        private bool IsNullable()
        {
            if (Index < _syntaxPattern.Length && _syntaxPattern[Index] == '*')
            {
                Index++;
                return true;
            }
            return false;
        }
    }
}
