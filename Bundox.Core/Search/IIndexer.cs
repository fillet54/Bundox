using System.Collections.Generic;

namespace Bundox.Core.Search
{
    public interface IIndexer<T>
        where T : Indexible
    {
        void AddToIndex(T item);
        IEnumerable<T> Retrieve(string query);
    }
}
