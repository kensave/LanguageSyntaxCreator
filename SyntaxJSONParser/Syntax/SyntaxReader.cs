using System;
using System.Collections.Generic;
using SyntaxJSONParser.Enums;
using System.Linq;

namespace SyntaxJSONParser.Syntax
{
    internal class SyntaxReader
    {
        private string _syntaxPattern;
        private LexerRepository _repository;
        public SyntaxReader(string syntaxPattern, LexerRepository repository)
        {
            _syntaxPattern = syntaxPattern.Replace(" ", "");
            _repository = repository;
        }
        public int Index { get; set; }

        public ISyntaxNode Take(bool takeUntilParent = false)
        {
            if (Index == _syntaxPattern.Length)
                return null;
            char node = _syntaxPattern[Index];
            Index++;
            bool takeUntil = takeUntilParent;
            if (node == '^')
            {
                node = _syntaxPattern[Index];
                Index++;
                takeUntil = true;
            }
            switch (node)
            {
                case '{':
                    return TakePlaceholder(takeUntil);
                case '<':
                    return TakeSequence(takeUntil);
                case '(':
                    return TakeGroup(takeUntil);
            }
            return null;
        }

        private ISyntaxNode TakeGroup(bool takeUntil)
        {
            var nodes = new List<ISyntaxNode>();
            bool isOr = false;
            while (_syntaxPattern[Index] != ')')
            {
                var innerNodes = new List<ISyntaxNode>();
                nodes.Add(Take(takeUntil));
                if (_syntaxPattern[Index] == '|')
                {
                    Index++;
                    isOr = true;
                }
            }
            Index++;
            var seq = new SyntaxNodeGroup()
            {
                Nodes = nodes,
                Nullable = IsNullable(),
                TakeUntil = takeUntil,
                IsOr = isOr,
                Alias = GetAlias()
            };
            return seq;
        }

        private ISyntaxNode TakeSequence(bool takeUntil)
        {
            var nodes = new List<ISyntaxNode>();
            while (_syntaxPattern[Index] != '>')
            {
                nodes.Add(Take(takeUntil));
                if (_syntaxPattern[Index] == ',')
                    Index++;
            }
            Index++;
            var seq = new SyntaxSequence()
            {
                Nodes = nodes,
                Nullable = IsNullable(),
                TakeUntil = takeUntil,
                Alias = GetAlias()
            };
            return seq;
        }

        private ISyntaxNode TakePlaceholder(bool takeUntil)
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
                return new SyntaxNodeKeyword()
                {
                    Name = name,
                    Nullable = nullable,
                    TakeUntil = takeUntil
                };
            }else
            {
                return new SyntaxNodeVariable()
                {
                    Name = name,
                    Nullable = nullable,
                    TakeUntil = takeUntil
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

        private string GetAlias()
        {
            string res = null;
            if (Index < _syntaxPattern.Length && _syntaxPattern[Index] == 'a')
            {
                Index+=2;
                
                while (Index < _syntaxPattern.Length && (!(new char[] { '{', '<', '(' }.ToList().Contains(_syntaxPattern[Index]))))
                {
                    res += _syntaxPattern[Index];
                    Index++;
                }
            }
            return res;
        }
    }
}
