namespace Storytel.Qml.Parsing
{
    using System.Collections.Generic;

    internal interface ILexer
    {
        List<Token> Tokenize(string source);
    }
}
