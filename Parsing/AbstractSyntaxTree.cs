namespace Pippi.Qml.Parsing
{
    using System.Collections.Generic;

    internal class AbstractSyntaxTree
    {
        public AbstractSyntaxTree() : this(new LinkedList<ParseNode>())
        {}

        public AbstractSyntaxTree(LinkedList<ParseNode> nodes)
        {
            Nodes = nodes;
        }

        public LinkedList<ParseNode> Nodes { get; }
    }
}
