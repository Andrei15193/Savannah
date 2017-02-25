namespace Savannah
{
    internal class StorageObjectProperty
    {
        internal StorageObjectProperty(string name, string value, ValueType propertyType)
        {
            Name = name;
            Value = value;
            Type = propertyType;
        }

        internal string Name { get; }

        internal string Value { get; }

        internal ValueType Type { get; }
    }
}