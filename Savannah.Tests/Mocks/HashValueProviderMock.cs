using System;
using Savannah.Utilities;

namespace Savannah.Tests.Mocks
{
    internal class HashValueProviderMock
        : IHashValueProvider
    {
        private readonly Func<string, string> _hashValueProvider;

        internal HashValueProviderMock(Func<string, string> hashValueProvider)
        {
            if (hashValueProvider == null)
                throw new ArgumentNullException(nameof(hashValueProvider));

            _hashValueProvider = hashValueProvider;
        }

        public string GetHashFor(string value)
            => _hashValueProvider(value);
    }
}