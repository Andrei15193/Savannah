using System.Xml;

namespace Savannah.Xml
{
    internal sealed class ObjectStoreXmlNameTable : NameTable
    {
        private static class NameTable
        {
            internal static XmlNameTable Instance { get; } = new ObjectStoreXmlNameTable();
        }

        internal static XmlNameTable Instance
            => NameTable.Instance;

        internal static string Bucket { get; } = NameTable.Instance.Add(nameof(Bucket));

        internal static string Partition { get; } = NameTable.Instance.Add(nameof(Partition));

        internal static string Object { get; } = NameTable.Instance.Add(nameof(Object));

        internal static string PartitionKey { get; } = NameTable.Instance.Add(nameof(PartitionKey));

        internal static string RowKey { get; } = NameTable.Instance.Add(nameof(RowKey));

        internal static string Timestamp { get; } = NameTable.Instance.Add(nameof(Timestamp));

        internal static string Value { get; } = NameTable.Instance.Add(nameof(Value));

        internal static string Type { get; } = NameTable.Instance.Add(nameof(Type));

        private ObjectStoreXmlNameTable()
        {
        }
    }
}