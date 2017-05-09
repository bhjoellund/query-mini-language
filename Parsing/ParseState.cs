namespace Storytel.Qml.Parsing
{
    internal enum ParseState
    {
        StartOfExpression,
        AfterField,
        AfterComparisonOperator,
        AfterValue,
        Error,
        Done
    }
}