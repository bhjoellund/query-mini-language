namespace Storytel.Qml.Parsing
{
    internal enum ComparisonOperator
    {
        Invalid,
        Equals,
        NotEquals,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        In
    }

    internal enum OperatorType
    {
        Invalid,
        Comparison,
        Boolean
    }
}