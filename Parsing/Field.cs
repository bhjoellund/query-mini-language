namespace Pippi.Qml.Parsing
{
    internal class Field
    {
        public Field(string name, QmlType type, string mapsTo)
        {
            Name = name;
            Type = type;
            MapsTo = mapsTo;
        }

        public string Name { get; }
        public QmlType Type { get; }
        public string MapsTo { get; }
    }
}
