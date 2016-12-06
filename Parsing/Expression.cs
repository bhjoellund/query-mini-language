namespace Pippi.Qml.Parsing
{
    internal class Expression : ParseNode
    {
        public Expression(Field field, Value value, ComparisonOperator @operator)
        {
            Field = field;
            Value = value;
            Operator = @operator;
        }

        public Field Field { get; }
        public Value Value { get; }
        public ComparisonOperator Operator { get; }
    }
}