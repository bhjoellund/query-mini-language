namespace Pippi.Qml.Parsing
{
    using System;

    internal static class LanguageProviderExtensions
    {
        public static Field TryGetField(this ILanguageProvider self, string name)
        {
            return Array.Find(self.KnownFields, f => f.Name == name);
        }

        public static ComparisonOperator TryGetComparisonOperator(this ILanguageProvider self, string op)
        {
            ComparisonOperator comparisonOperator;

            if (!self.KnownComparisonOperators.TryGetValue(op, out comparisonOperator))
                comparisonOperator = ComparisonOperator.Invalid;

            return comparisonOperator;
        }

        public static OperatorType GetOperatorType(this ILanguageProvider self, string op)
        {
            if(self.KnownComparisonOperators.ContainsKey(op))
                return OperatorType.Comparison;

            if(self.KnownBooleanOperators.ContainsKey(op))
                return OperatorType.Boolean;

            return OperatorType.Invalid;
        }

        public static BooleanOperator GetBooleanOperator(this ILanguageProvider self, string op)
        {
            BooleanOperator booleanOperator;

            if (!self.KnownBooleanOperators.TryGetValue(op, out booleanOperator))
                booleanOperator = BooleanOperator.Invalid;

            return booleanOperator;
        }
    }
}
