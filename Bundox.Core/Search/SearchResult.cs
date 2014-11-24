using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bundox.Core.Search
{
    public class SearchResult<TNode>
    {
        public TNode Node { get; private set; }

        public List<Tuple<int, int>> MatchRanges { get; private set; }

        public SearchResult(TNode node, List<Tuple<int, int>> matchRanges)
        {
            Node = node;
            MatchRanges = matchRanges;
        }
    }
}
