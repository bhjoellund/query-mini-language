namespace Pippi.Qml.Parsing
{
    using System.Collections.Generic;

    internal interface ILanguageProvider
    {
        Dictionary<string, ComparisonOperator> KnownComparisonOperators { get; }
        Dictionary<string, BooleanOperator> KnownBooleanOperators { get; }
        Field[] KnownFields { get; }
        string[] KnownDateTimeFormats { get; }
    }
}