using Savannah.FileSystem;
using Savannah.Tests.Mocks;
using Savannah.Tests.Utilities;
using Savannah.Utilities;

namespace Savannah
{
    public partial class ObjectStore
    {
        private static readonly IHashValueProvider _defaultHashValueProvider = new Md5HashValueProvider();
        private static readonly IFileSystem _defaultFileSystem = new FileSystemMock();

        internal ObjectStore(string storageFolderName)
            : this(storageFolderName, _defaultHashValueProvider, _defaultFileSystem)
        {
        }
    }
}