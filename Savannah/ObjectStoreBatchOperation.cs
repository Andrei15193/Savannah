using System;
using System.Collections;
using System.Collections.Generic;

namespace Savannah
{
    public sealed class ObjectStoreBatchOperation : IList<ObjectStoreOperation>
    {
        private readonly IList<ObjectStoreOperation> _operations;

        public ObjectStoreBatchOperation()
        {
            _operations = new List<ObjectStoreOperation>();
        }

        public void Delete(object @object)
            => Add(ObjectStoreOperation.Delete(@object));

        public void Insert(object @object)
            => Add(ObjectStoreOperation.Insert(@object));

        public void Insert(object @object, bool echoContent)
            => Add(ObjectStoreOperation.Insert(@object, echoContent));

        public void InsertOrMerge(object @object)
            => Add(ObjectStoreOperation.InsertOrMerge(@object));

        public void InsertOrReplace(object @object)
            => Add(ObjectStoreOperation.InsertOrReplace(@object));

        public void Retrieve(object @object)
            => Add(ObjectStoreOperation.Retrieve(@object));

        public void Retrieve(object @object, IEnumerable<string> propertiesToRetrieve)
            => Add(ObjectStoreOperation.Retrieve(@object, propertiesToRetrieve));

        public void Retrieve(object @object, params string[] propertiesToRetrieve)
            => Add(ObjectStoreOperation.Retrieve(@object, propertiesToRetrieve));

        public void Retrieve<T>(object @object) where T : new()
            => Add(ObjectStoreOperation.Retrieve<T>(@object));

        public void Retrieve<T>(object @object, IEnumerable<string> propertiesToRetrieve) where T : new()
            => Add(ObjectStoreOperation.Retrieve<T>(@object, propertiesToRetrieve));

        public void Retrieve<T>(object @object, params string[] propertiesToRetrieve) where T : new()
            => Add(ObjectStoreOperation.Retrieve<T>(@object, propertiesToRetrieve));

        public void Retrieve<T>(object @object, ObjectResolver<T> objectResolver)
            => Add(ObjectStoreOperation.Retrieve<T>(@object, objectResolver));

        public void Retrieve<T>(object @object, ObjectResolver<T> objectResolver, IEnumerable<string> propertiesToRetrieve)
            => Add(ObjectStoreOperation.Retrieve<T>(@object, objectResolver, propertiesToRetrieve));

        public void Retrieve<T>(object @object, ObjectResolver<T> objectResolver, params string[] propertiesToRetrieve)
            => Add(ObjectStoreOperation.Retrieve<T>(@object, objectResolver, propertiesToRetrieve));

        public ObjectStoreOperation this[int index]
        {
            get
            {
                return _operations[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _operations[index] = value;
            }
        }

        public int Count
            => _operations.Count;

        public bool IsReadOnly
            => _operations.IsReadOnly;

        public void Add(ObjectStoreOperation objectStoreOperation)
        {
            if (objectStoreOperation == null)
                throw new ArgumentNullException(nameof(objectStoreOperation));

            _operations.Add(objectStoreOperation);
        }

        public void Clear()
            => _operations.Clear();

        public bool Contains(ObjectStoreOperation objectStoreOperation)
            => (objectStoreOperation != null && _operations.Contains(objectStoreOperation));

        public void CopyTo(ObjectStoreOperation[] array, int arrayIndex)
            => _operations.CopyTo(array, arrayIndex);

        public IEnumerator<ObjectStoreOperation> GetEnumerator()
            => _operations.GetEnumerator();

        public int IndexOf(ObjectStoreOperation objectStoreOperation)
            => (objectStoreOperation == null ? -1 : _operations.IndexOf(objectStoreOperation));

        public void Insert(int index, ObjectStoreOperation objectStoreOperation)
        {
            if (objectStoreOperation == null)
                throw new ArgumentNullException(nameof(objectStoreOperation));

            _operations.Insert(index, objectStoreOperation);
        }

        public bool Remove(ObjectStoreOperation objectStoreOperation)
            => (objectStoreOperation != null && _operations.Remove(objectStoreOperation));

        public void RemoveAt(int index)
            => _operations.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}