using System.Collections.Generic;

namespace Bundox.Core.Search
{
    public interface ISuffixIndex<T>
        where T : Indexible
    {
        IEnumerable<T> Retrieve(params string[] substrings);
    }
}
