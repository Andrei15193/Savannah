using Savannah.FileSystem;
using Savannah.Utilities;
using Savannah.WindowsUniversalPlatform.FileSystem;
using Savannah.WindowsUniversalPlatform.Utilities;

namespace Savannah
{
    public partial class ObjectStore
    {
        private static readonly IHashValueProvider _defaultHashValueProvider = new Md5HashValueProvider();
        private static readonly IFileSystem _defaultFileSystem = new WindowsUniversalPlatformFileSystem();

        public ObjectStore(string storageFolderName)
            : this(storageFolderName, _defaultHashValueProvider, _defaultFileSystem)
        {
        }
    }
}