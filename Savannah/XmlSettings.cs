using System.Text;
using System.Xml;

namespace Savannah
{
    internal static class XmlSettings
    {
        internal static string DateTimeFormat { get; } = "yyyy/MM/dd HH:mm:ss:fffffffK";

        internal static XmlReaderSettings ReaderSettings { get; } = new XmlReaderSettings
        {
            Async = true,
            CheckCharacters = true,
            CloseInput = true,
            ConformanceLevel = ConformanceLevel.Document,
            DtdProcessing = DtdProcessing.Ignore,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            NameTable = ObjectStoreXmlNameTable.Instance
        };

        internal static XmlWriterSettings WriterSettings { get; } = new XmlWriterSettings
        {
            Async = true,
            CheckCharacters = true,
            CloseOutput = true,
            ConformanceLevel = ConformanceLevel.Document,
            Encoding = Encoding.UTF8,
            Indent = false,
            IndentChars = string.Empty,
            NamespaceHandling = NamespaceHandling.OmitDuplicates,
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace,
            NewLineOnAttributes = false,
            OmitXmlDeclaration = false,
            WriteEndDocumentOnClose = true
        };
    }
}