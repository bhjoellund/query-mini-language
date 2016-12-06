namespace Pippi.Qml.Parsing
{
    using System;

    internal class Value
    {
        private Value() { }

        public object ContainedValue { get; set; }
        public QmlType Type { get; private set; }
        public string Token { get; private set; }

        public static Value NewString(string value, string token)
        {
            return new Value
            {
                ContainedValue = value,
                Token = token,
                Type = QmlType.String
            };
        }

        public static Value NewList(Value[] values, string token)
        {
            return new Value
            {
                ContainedValue = values,
                Token = token,
                Type = QmlType.List
            };
        }

        public static Value NewInteger(int value, string token)
        {
            return new Value
            {
                ContainedValue = value,
                Token = token,
                Type = QmlType.Integer
            };
        }

        public static Value NewFloat(float value, string token)
        {
            return new Value
            {
                ContainedValue = value,
                Token = token,
                Type = QmlType.Float
            };
        }

        public static Value NewDateTime(DateTime dateTime, string token)
        {
            return new Value
            {
                ContainedValue = dateTime,
                Token = token,
                Type = QmlType.DateTime
            };
        }

        public static Value NewTimeSpan(TimeSpan timeSpan, string token)
        {
            return new Value
            {
                ContainedValue = timeSpan,
                Token = token,
                Type = QmlType.TimeSpan
            };
        }
    }
}
