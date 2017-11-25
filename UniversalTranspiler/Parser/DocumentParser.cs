using System;
using UniversalTranspiler.Interfaces;
using UniversalTranspiler.Syntax;

namespace UniversalTranspiler
{
    public class DocumentParser : ILanguajeParser
    {
        private ParseableTokenStream TokenStream { get; set; }
        private Enums.Languaje _languaje;
        private LexerRepository _repository { get; set; }


        public DocumentParser(string source, Enums.Languaje lang)
        {
            _repository = new LexerRepository(lang);
            TokenStream = new ParseableTokenStream(new LexerTokenizer(source, lang));
            _languaje = lang;
        }

        public object Parse()
        {
            return ParseSyntax("Document");
        }

        private object ParseSyntax(string syntaxNodeStr)
        {
            var syntaxNode = _repository.GetSyntaxPattern(syntaxNodeStr);
            var syntaxReader = new SyntaxReader(syntaxNode, _languaje);
            var node = syntaxReader.Take();
            while (node != null)
            {
                Parse(node);
                node = syntaxReader.Take();
            }
            return null;
        }
        private object Parse(ISyntaxNode seq)
        {
            if (seq is SyntaxNodeVariable)
                Parse(seq as SyntaxNodeVariable);
            else if (seq is SyntaxSequence)
                Parse(seq as SyntaxSequence);
            else if (seq is SyntaxNodeKeyword)
                Parse(seq as SyntaxNodeKeyword);
            else if (seq is SyntaxNodeOr)
                Parse(seq as SyntaxNodeOr);
            return null;
        }
        private object Parse(SyntaxSequence seq)
        {
            foreach (var node in seq.Nodes)
            {
                Parse(node);
            }
            return null;
        }
        private object Parse(SyntaxNodeOr seq)
        {
            foreach (var node in seq.Nodes)
            {
                if (Parse(node) != null)
                    break;
            }
            return null;
        }
        
        private object Parse(SyntaxNodeVariable node)
        {
            return ParseSyntax(node.Name);
        }
        private object Parse(SyntaxNodeKeyword node)
        {
            var nod = TokenStream.Take(node.Name);
            if (nod != null)
            {
                return nod;
            }
            return null;
        }
    }
    
}
