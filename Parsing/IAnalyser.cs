namespace Storytel.Qml.Parsing
{
    using System.Collections.Generic;

    internal interface IAnalyser
    {
        AbstractSyntaxTree Analyse(List<Token> tokens);
    }
}