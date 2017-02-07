using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Savannah.Utilities;

namespace Savannah.Tests.Utilities
{
    [TestClass]
    public class Md5HashProviderTests
    {
        [DataTestMethod]
        [DataRow("test value", "cc2d2adc8b1da820c1075a099866ceb4")]
        [DataRow("test", "098f6bcd4621d373cade4e832627b4f6")]
        [DataRow("", "d41d8cd98f00b204e9800998ecf8427e")]
        [DataRow(default(string), "d41d8cd98f00b204e9800998ecf8427e")]
        public void TestGetMd5Hash(string value, string expectedHash)
        {
            var hashProvider = new Md5HashProvider();

            var actualHash = hashProvider.GetHashFor(value);

            Assert.AreEqual(expectedHash, actualHash, ignoreCase: false);
        }
    }
}