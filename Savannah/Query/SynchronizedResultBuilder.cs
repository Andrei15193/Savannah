using System.Collections.Generic;
using System.Linq;

namespace Savannah.Query
{
    internal class SynchronizedResultBuilder
        : IResultBuilder
    {
        private readonly ResultBuilder _resultBuilder;

        internal SynchronizedResultBuilder(int? maxCount)
        {
            _resultBuilder = new ResultBuilder(maxCount);
        }

        public IEnumerable<StorageObject> Result
        {
            get
            {
                lock (_resultBuilder)
                    return _resultBuilder.Result.ToList();
            }
        }

        public bool TryAdd(StorageObject @object)
        {
            lock (_resultBuilder)
                return _resultBuilder.TryAdd(@object);
        }
    }
}