namespace Pippi.Qml
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Parsing;

    internal class SolrCodeGenerator : ICodeGenerator<AbstractSyntaxTree, string>
    {
        public string Generate(AbstractSyntaxTree input)
        {
            var sb = new StringBuilder();

            foreach (var node in input.Nodes)
            {
                var expression = node as Expression;
                if (expression != null)
                {
                    var solrField = expression.Field.MapsTo;
                    var op = expression.Operator;
                    var value = FormatValue(expression.Value);

                    switch (op)
                    {
                        case ComparisonOperator.Equals:
                            sb.Append($"+{solrField}:{value} ");
                            break;
                        case ComparisonOperator.NotEquals:
                            sb.Append($"-{solrField}:{value} ");
                            break;
                        case ComparisonOperator.LessThan:
                            sb.Append($"+{solrField}:[* TO {value}}} ");
                            break;
                        case ComparisonOperator.LessThanOrEqual:
                            sb.Append($"+{solrField}:[* TO {value}] ");
                            break;
                        case ComparisonOperator.GreaterThan:
                            sb.Append($"+{solrField}:{{{value} TO *] ");
                            break;
                        case ComparisonOperator.GreaterThanOrEqual:
                            sb.Append($"+{solrField}:[{value} TO *] ");
                            break;
                        case ComparisonOperator.In:
                            sb.Append($"+{solrField}:({value}) ");
                            break;
                    }
                }
                else
                {
                    var subExpression = node as SubExpression;

                    if (subExpression != null)
                        sb.Append($"+({Generate(new AbstractSyntaxTree(subExpression.Nodes))})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static string FormatValue(Value value)
        {
            switch (value.Type)
            {
                case QmlType.DateTime:
                {
                    var datetime = TrimDateTime((DateTime)value.ContainedValue, false);
                    var utc = datetime.ToUniversalTime();
                    return utc.ToString("o", CultureInfo.InvariantCulture);
                }
                case QmlType.TimeSpan:
                {
                    var timeSpan = (TimeSpan)value.ContainedValue;
                    var utc = TrimDateTime(DateTime.UtcNow, true) + timeSpan;
                    return utc.ToString("s", CultureInfo.InvariantCulture) + "Z";
                }
                case QmlType.Float:
                    return ((float)value.ContainedValue).ToString(CultureInfo.InvariantCulture);
                case QmlType.List:
                    var arr = (object[]) value.ContainedValue;
                    return String.Join(" ", arr.Select(obj => ((Value) obj).ContainedValue));
                default:
                    return value.ContainedValue.ToString();
            }
        }

        private static DateTime TrimDateTime(DateTime dateTime, bool dateOnly)
        {
            if(!dateOnly)
                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }
}