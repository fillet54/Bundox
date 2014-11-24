using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bundox.Core.Search
{
    public interface ISuggester<T>
    {
        IEnumerable<SearchResult<T>> SuggestionsFor(string query);
    }
}
