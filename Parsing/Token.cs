namespace Pippi.Qml.Parsing
{
    internal class Token
    {
        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; }
        public TokenType Type { get; }

        public override string ToString()
        {
            return $"{Value} ({Type})";
        }
    }

    internal enum TokenType
    {
        Identifier,
        String,
        Number,
        DateTime,
        Operator,
        Delimiter,
        TimeSpan
    }
}