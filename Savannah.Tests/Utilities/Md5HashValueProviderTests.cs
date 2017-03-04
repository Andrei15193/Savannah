using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Savannah.Tests.Utilities
{
    [TestClass]
    public class Md5HashValueProviderTests
        : UnitTest
    {
        [TestMethod]
        [DeploymentItem(DataFilePath)]
        [DataSource(DataProviderName, DataFileName, Md5HashValuesTable, DataAccessMethod.Sequential)]
        [Owner("Andrei Fangli")]
        public void TestGetMd5Hash()
        {
            var row = GetRow<Md5HashValuesRow>();

            var value = row.StringValue;
            var expectedHashValue = row.HashValue;

            var hashValueProvider = new Md5HashValueProvider();

            var actualHashValue = hashValueProvider.GetHashFor(value);

            Assert.AreEqual(expectedHashValue, actualHashValue, ignoreCase: true);
        }
    }
}