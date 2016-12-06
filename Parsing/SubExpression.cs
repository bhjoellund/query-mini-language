namespace Pippi.Qml.Parsing
{
    using System.Collections.Generic;

    internal class SubExpression : ParseNode
    {
        public SubExpression(LinkedList<ParseNode> nodes)
        {
            Nodes = nodes;
        }

        public LinkedList<ParseNode> Nodes { get; }
    }
}