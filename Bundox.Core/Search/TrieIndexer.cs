using Gma.DataStructures.StringSearch;
using System.Collections.Generic;
using System.Linq;

namespace Bundox.Core.Search
{
    public class TrieIndexer<TNode> : ISuffixIndexer<TNode>
        where TNode : Indexible
    {
        private ITrie<TNode> indexer = new PatriciaSuffixTrie<TNode>(3);

        public void AddToIndex(params TNode[] items)
        {
            items.ToList().ForEach(item =>
            {
                indexer.Add(item.IndexOn.ToLower(), item);
            });
        }

        public IEnumerable<TNode> Retrieve(params string[] substrings)
        {
            return indexer.Retrieve(substrings.First());
        }
    }
}
