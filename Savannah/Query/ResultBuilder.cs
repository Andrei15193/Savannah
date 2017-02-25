using System;
using System.Collections.Generic;

namespace Savannah.Query
{
    internal class ResultBuilder
        : IResultBuilder
    {
        private readonly int? _maxCount;
        private readonly List<StorageObject> _result;

        internal ResultBuilder(int? maxCount = null)
        {
            _maxCount = maxCount;
            _result = new List<StorageObject>();
        }

        public virtual IEnumerable<StorageObject> Result
            => _result;

        public virtual bool TryAdd(StorageObject @object)
        {
#if DEBUG
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));
#endif
            int indertIndex = _FindInsertIndex(@object);
            return _TryInsert(indertIndex, @object);
        }

        private bool _TryInsert(int indertIndex, StorageObject @object)
        {
            if (indertIndex < _result.Count)
            {
                if (_maxCount != null && _result.Count == _maxCount.Value)
                    _result.RemoveAt(_result.Count - 1);
                _result.Insert(indertIndex, @object);
                return true;
            }
            else
            {
                if (_maxCount == null || _result.Count < _maxCount.Value)
                {
                    _result.Add(@object);
                    return true;
                }

                return false;
            }
        }

        private int _FindInsertIndex(StorageObject @object)
        {
            var index = 0;
            var found = false;

            while (!found && index < _result.Count)
            {
                var partitionKeyComparison = ObjectStoreLimitations.StringComparer.Compare(@object.PartitionKey, _result[index].PartitionKey);
                if (partitionKeyComparison > 0)
                    index++;
                else if (partitionKeyComparison == 0)
                {
                    var rowKeyComparison = ObjectStoreLimitations.StringComparer.Compare(@object.RowKey, _result[index].RowKey);
                    if (rowKeyComparison > 0)
                        index++;
                    else
                        found = true;
                }
                else
                    found = true;
            }

            return index;
        }
    }
}