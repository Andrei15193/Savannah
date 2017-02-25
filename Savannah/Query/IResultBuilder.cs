using System.Collections.Generic;

namespace Savannah.Query
{
    internal interface IResultBuilder
    {
        bool TryAdd(StorageObject @object);

        IEnumerable<StorageObject> Result { get; }
    }
}