using System.Collections.Generic;

namespace Savannah
{
    public class ObjectStoreQuery
    {
        public ObjectStoreQuery()
        {
        }

        public ObjectStoreQueryFilter Filter { get; set; }

        public int? Take { get; set; }

        public IEnumerable<string> Properties { get; set; }
    }
}