using System.Security.Cryptography;
using System.Text;
using Savannah.Utilities;

namespace Savannah.Tests.Utilities
{
    internal sealed class Md5HashValueProvider
        : IHashValueProvider
    {
        public string GetHashFor(string value)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.Default.GetBytes(value ?? string.Empty);
            var hashBytes = md5.ComputeHash(inputBytes);

            var resultBuilder = new StringBuilder();
            foreach (var hashByte in hashBytes)
                resultBuilder.Append(hashByte.ToString("X2"));

            return resultBuilder.ToString();
        }
    }
}