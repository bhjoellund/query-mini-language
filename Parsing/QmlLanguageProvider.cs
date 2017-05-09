namespace Storytel.Qml.Parsing
{
    using System.Collections.Generic;

    internal class QmlLanguageProvider : ILanguageProvider
    {
        public Dictionary<string, ComparisonOperator> KnownComparisonOperators { get; } = new Dictionary<string, ComparisonOperator>
            {
                {"=",  ComparisonOperator.Equals},
                {"!=", ComparisonOperator.NotEquals},
                {"<",  ComparisonOperator.LessThan},
                {">",  ComparisonOperator.GreaterThan},
                {"<=", ComparisonOperator.LessThanOrEqual},
                {">=", ComparisonOperator.GreaterThanOrEqual},
                {"~=", ComparisonOperator.In}
            };

        public Dictionary<string, BooleanOperator> KnownBooleanOperators { get; } = new Dictionary<string, BooleanOperator>
            {
                {"&&", BooleanOperator.And }
            };

        public Field[] KnownFields { get; } = {
                new Field("author_id", QmlType.Integer, "contributorids_string_mv"),
                new Field("pub_date", QmlType.DateTime, "publicationdate_date"),
                new Field("kpi", QmlType.Float, "kpi_float"),
                new Field("publisher_id", QmlType.Integer, "publisher_int"),
                new Field("title", QmlType.String, "title_string"),
                new Field("language", QmlType.String, "language_string"),
                new Field("type", QmlType.String, "discriminator_string"),
                new Field("price", QmlType.Float, "mprice_float"),
                new Field("series_id", QmlType.Integer, "seriesid_int"),
                new Field("bic_genre_id", QmlType.Integer, "bicgenreids_int_mv"),
                new Field("book_id", QmlType.Integer, "tableid_string")
            };

        public string[] KnownDateTimeFormats { get; } = { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" };
    }
}
