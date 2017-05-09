namespace Storytel.Qml
{
    using System;
    using Parsing;

    internal class QmlCompiler : ICompiler<string, string>
    {
        private readonly ILexer _lexer;
        private readonly IAnalyser _analyser;
        private readonly ICodeGenerator<AbstractSyntaxTree, string> _codeGenerator;

        public QmlCompiler(ILexer lexer, IAnalyser analyser, ICodeGenerator<AbstractSyntaxTree, string> codeGenerator)
        {
            _lexer = lexer;
            _analyser = analyser;
            _codeGenerator = codeGenerator;
        }

        public string Compile(string qml)
        {
            if (String.IsNullOrWhiteSpace(qml))
                return null;

            var tokens = _lexer.Tokenize(qml);
            var ast = _analyser.Analyse(tokens);
            
            return _codeGenerator.Generate(ast);
        }
    }
}
