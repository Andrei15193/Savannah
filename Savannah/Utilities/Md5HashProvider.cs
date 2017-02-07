using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Savannah.Utilities
{
    internal sealed class Md5HashProvider
        : IHashProvider
    {
        private static readonly HashAlgorithmProvider _hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);

        public string GetHashFor(string value)
        {
            var buffer = CryptographicBuffer.ConvertStringToBinary((value ?? string.Empty), BinaryStringEncoding.Utf8);

            var hashResult = _hashAlgorithm.HashData(buffer);
            var hexEncodedResult = CryptographicBuffer.EncodeToHexString(hashResult);

            return hexEncodedResult;
        }
    }
}