using Gma.DataStructures.StringSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bundox.Core.Extensions;

namespace Bundox.Core.Search
{
    public class FuzzySuggester<TNode> : ISuggester<TNode>, IIndexer<TNode> 
        where TNode : Indexible 
    {
        private ITrie<TNode> indexer = new PatriciaSuffixTrie<TNode>(3);

        public IEnumerable<SearchResult<TNode>> SuggestionsFor(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Enumerable.Empty<SearchResult<TNode>>();
            }

            Func<string, string, int> closenessToBeginning = (s, q) => normalize(s).IndexOf(normalize(q));

            IEnumerable<SearchResult<TNode>> suggestions = 
                Retrieve(query)
                .Select(n => new SearchResult<TNode>(n, GetNormalizedMatchedRanges(n.IndexOn, new List<string>{query})))
                .OrderBy(s => closenessToBeginning(s.Node.IndexOn, query));


            int MAX_PARTITIONS = 4;
            for (int i = 1; i < MAX_PARTITIONS; i++)
            {
                suggestions = suggestions
                              .Concat(query.Partition(i + 1)
                                           .Select(p => new { partitions = p, matches = Retrieve(p.OrderByDescending(s => s.Length).First()) })
                                           .SelectMany(p => p.matches
                                                            .Select(n => new SearchResult<TNode>(n, GetNormalizedMatchedRanges(n.IndexOn, p.partitions)))
                                                            .Where(sr => sr.MatchRanges.Count() > 0)));
            }
            return suggestions; 
        }

        private List<Tuple<int, int>> GetNormalizedMatchedRanges(string subject, IEnumerable<string> substrings)
        {
            var ranges = new List<Tuple<int, int>>();

            var subjectLocation = 0;
            var whatsLeft = normalize(subject);
            foreach (var substring in substrings.Select(normalize))
            {
                if (whatsLeft.Contains(substring))
                {
                    subjectLocation += whatsLeft.IndexOf(substring);
                    ranges.Add(Tuple.Create(subjectLocation, substring.Length)); 
                    whatsLeft = whatsLeft.Substring(whatsLeft.IndexOf(substring) + substring.Length);
                    subjectLocation += substring.Length;
                }
                else
                {
                    return new List<Tuple<int, int>>();
                }
            }
            return ranges;
        }

        private bool containsNormalizedSubstringsInOrder(string subject, IEnumerable<string> substrings)
        {
            return GetNormalizedMatchedRanges(subject, substrings).Count() != 0; 
        }

        private bool containsNormalizedSubstringsInOrder(string subject, string prefix, string suffix)
        {
            return containsSubstringsInOrder(normalize(subject), normalize(prefix), normalize(suffix));
        }
        
        private bool containsSubstringsInOrder(string subject, string prefix, string suffix)
        {
            var endOfPrefixLocation = subject.IndexOf(prefix) + prefix.Length;
            var startOfSuffixLocation = subject.IndexOf(suffix);
            return endOfPrefixLocation < startOfSuffixLocation;
        }


        public IEnumerable<Tuple<string, string>> SplitString(string original)
        {
            if (original.Length <= 1) 
                return new List<Tuple<string, string>> { Tuple.Create (original, "") };

            return from i in Enumerable.Range(1, original.Length - 1)
                   let splitPoint = original.Length - i
                   select Tuple.Create(original.Substring(0, splitPoint),
                                       original.Substring(splitPoint));
        }

        public void AddToIndex(TNode item)
        {
            indexer.Add(normalize(item.IndexOn), item);
        }

        public IEnumerable<TNode> Retrieve(string query)
        {
            return indexer.Retrieve(normalize(query));
        }
        
        private string normalize(string original)
        {
            return original.ToLower();
        }

    }
}
