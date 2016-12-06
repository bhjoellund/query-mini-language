namespace Pippi.Qml.Parsing
{
    internal class BooleanOperatorNode : ParseNode
    {
        public BooleanOperatorNode(BooleanOperator booleanOperator)
        {
            Type = booleanOperator;
        }

        public BooleanOperator Type { get; }
    }
}