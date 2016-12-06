namespace Pippi.Qml.Parsing
{
    using System;
    using System.Text.RegularExpressions;

    internal static class TimeSpanTranslator
    {
        private static readonly Regex TimeSpanPattern = new Regex(@"^(?'range'-?\d+)\s+(?'designator'month|day|week|year)s?|today$");

        public static TimeSpan Translate(string token)
        {
            var match = TimeSpanPattern.Match(token);

            if(!match.Success)
                throw new ParseException($"Unrecognized timespan pattern: {token}");

            if (!match.Groups["range"].Success)
                return TimeSpan.Zero;

            var range = Int32.Parse(match.Groups["range"].Value);
            var designator = match.Groups["designator"].Value;

            return GetTimeSpanFromRangeAndDesignator(range, designator);
        }

        private static TimeSpan GetTimeSpanFromRangeAndDesignator(int range, string designator)
        {
            switch (designator)
            {
                case "year":
                    return DateTime.UtcNow.AddYears(range) - DateTime.UtcNow;
                case "month":
                    return DateTime.UtcNow.AddMonths(range) - DateTime.UtcNow;
                case "week":
                    return TimeSpan.FromDays(range*7);
                default:
                    return TimeSpan.FromDays(range);
            }
        }
    }
}
