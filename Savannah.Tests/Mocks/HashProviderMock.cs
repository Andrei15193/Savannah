using System;
using Savannah.Utilities;

namespace Savannah.Tests.Mocks
{
    internal class HashProviderMock
        : IHashProvider
    {
        private readonly Func<string, string> _hashProvider;

        internal HashProviderMock(Func<string, string> hashProvider)
        {
            if (hashProvider == null)
                throw new ArgumentNullException(nameof(hashProvider));

            _hashProvider = hashProvider;
        }

        public string GetHashFor(string value)
            => _hashProvider(value);
    }
}