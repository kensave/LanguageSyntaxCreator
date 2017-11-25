using System;
using System.Collections.Generic;
using UniversalTranspiler.Enums;

namespace UniversalTranspiler.Syntax
{
    internal class SyntaxReader
    {
        private string _syntaxPattern;
        private LexerRepository _repository;
        private Languaje _language;
        public SyntaxReader(string syntaxPattern, Languaje lang)
        {
            _syntaxPattern = syntaxPattern.Replace(" ", "");
            _language = lang;
            _repository = new LexerRepository(lang);
        }
        public int Index { get; set; }

        public ISyntaxNode Take()
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

        private ISyntaxNode TakeGroup()
        {
            var nodes = new List<ISyntaxNode>();
            while (_syntaxPattern[Index] != ')')
            {
                nodes.Add(Take());
                if (_syntaxPattern[Index] == '|')
                    Index++;
            }
            Index++;
            var seq = new SyntaxNodeOr()
            {
                Nodes = nodes,
                Nullable = IsNullable()
            };
            return seq;
        }

        private ISyntaxNode TakeSequence()
        {
            var nodes = new List<ISyntaxNode>();
            while (_syntaxPattern[Index] != '>')
            {
                nodes.Add(Take());
                if (_syntaxPattern[Index] == ',')
                    Index++;
            }
            Index++;
            var seq = new SyntaxSequence()
            {
                Nodes = nodes,
                Nullable = IsNullable()
            };
            return seq;
        }

        private ISyntaxNode TakePlaceholder()
        {
            var name = "";
            while (_syntaxPattern[Index] != '}')
            {
                name += _syntaxPattern[Index];
                Index++;
            }
            bool nullable = IsNullable();
            Index++;
            if (_repository.IsSpecialCharacter(name) || _repository.IsKeyword(name))
            {
                return new SyntaxNodeKeyword()
                {
                    Name = name,
                    Nullable = nullable
                };
            }else
            {
                return new SyntaxNodeVariable()
                {
                    Name = name,
                    Nullable = nullable
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
