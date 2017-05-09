namespace Storytel.Qml.Parsing
{
    using System;

    internal class ParseException : Exception
    {
        public ParseException(string message) : base(message) { }
    }
}