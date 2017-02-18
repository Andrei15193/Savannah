using System;
using System.Collections.Generic;
using Savannah.ObjectStoreOperations;

namespace Savannah
{
    public delegate T ObjectResolver<T>(string partitionKey, string rowKey, DateTime timestamp, IReadOnlyDictionary<string, object> properyValues);

    public abstract class ObjectStoreOperation
    {
        public static ObjectStoreOperation Delete(object @object)
            => new DeleteObjectStoreOperation(@object);

        public static ObjectStoreOperation Insert(object @object)
            => new InsertObjectStoreOperation(@object);

        public static ObjectStoreOperation Insert(object @object, bool echoContent)
            => new InsertObjectStoreOperation(@object, echoContent);

        public static ObjectStoreOperation InsertOrMerge(object @object)
            => new InsertOrMergeObjectStoreOperation(@object);

        public static ObjectStoreOperation InsertOrReplace(object @object)
            => new InsertOrReplaceObjectStoreOperation(@object);

        public static ObjectStoreOperation Merge(object @object)
            => new MergeObjectStoreOperation(@object);

        public static ObjectStoreOperation Retrieve(object @object)
            => new RetrieveDynamicObjectStoreOperation(@object);

        public static ObjectStoreOperation Retrieve(object @object, IEnumerable<string> propertiesToRetrieve)
            => new RetrieveDynamicObjectStoreOperation(@object, propertiesToRetrieve);

        public static ObjectStoreOperation Retrieve(object @object, params string[] propertiesToRetrieve)
            => new RetrieveDynamicObjectStoreOperation(@object, propertiesToRetrieve);

        public static ObjectStoreOperation Retrieve<T>(object @object) where T : new()
            => new RetrievePocoObjectStoreOperation<T>(@object);

        public static ObjectStoreOperation Retrieve<T>(object @object, IEnumerable<string> propertiesToRetrieve) where T : new()
            => new RetrievePocoObjectStoreOperation<T>(@object, propertiesToRetrieve);

        public static ObjectStoreOperation Retrieve<T>(object @object, params string[] propertiesToRetrieve) where T : new()
            => new RetrievePocoObjectStoreOperation<T>(@object, propertiesToRetrieve);

        public static ObjectStoreOperation Retrieve<T>(object @object, ObjectResolver<T> objectResolver)
            => new RetrieveDelegateObjectStoreOperation<T>(@object, objectResolver);

        public static ObjectStoreOperation Retrieve<T>(object @object, ObjectResolver<T> objectResolver, IEnumerable<string> propertiesToRetrieve)
            => new RetrieveDelegateObjectStoreOperation<T>(@object, objectResolver, propertiesToRetrieve);

        public static ObjectStoreOperation Retrieve<T>(object @object, ObjectResolver<T> objectResolver, params string[] propertiesToRetrieve)
            => new RetrieveDelegateObjectStoreOperation<T>(@object, objectResolver, propertiesToRetrieve);

        internal ObjectStoreOperation(object @object)
        {
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));

            Object = @object;

            Metadata = ObjectMetadata.GetFor(@object.GetType());
            PartitionKey = (string)Metadata?.PartitionKeyProperty?.GetValue(@object);
            RowKey = (string)Metadata?.RowKeyProperty?.GetValue(@object);
        }

        public object Object { get; }

        public abstract ObjectStoreOperationType OperationType { get; }

        internal ObjectMetadata Metadata { get; }

        internal string PartitionKey { get; }

        internal string RowKey { get; }

        internal abstract StorageObject GetStorageObjectFrom(ObjectStoreOperationExectionContext context);
    }
}