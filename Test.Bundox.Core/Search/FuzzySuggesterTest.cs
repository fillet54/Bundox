using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Bundox.Core.Search;
using Bundox.Core.Search;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Test.Bundox.Core.Search
{
    public class TestNode : Indexible
    {
        public TestNode(string indexOn)
        {
            IndexOn = indexOn;
        }

        public string IndexOn
        {
            get; 
            private set;
        }
    }

    [TestClass]
    public class FuzzySuggesterTest
    {
        private FuzzySuggester<TestNode> suggester;
        private ISuffixIndexer<TestNode> indexer;

        [TestInitialize]
        public void setup()
        {
            indexer = new TrieIndexer<TestNode>();
            suggester = new FuzzySuggester<TestNode>(indexer);
        }

        [TestMethod]
        public void nullQueryReturnsAnEmptySet()
        {
            Assert.AreEqual(0, suggester.SuggestionsFor(null).Count());
        }
        
        [TestMethod]
        public void emptyStringQueryReturnsAnEmptySet()
        {
            Assert.AreEqual(0, suggester.SuggestionsFor("").Count());
        }

        [TestMethod]
        public void suggestionsForFullQueryStringMatchAreReturned() 
        {
            var bananaNode = new TestNode("banana");
            var bandanaNode = new TestNode("bandana");
            indexer.AddToIndex(bananaNode);
            indexer.AddToIndex(bandanaNode);

            AssertContainsSearchResult(suggester.SuggestionsFor("band"), bandanaNode);
            AssertDoesNotContainSearchResult(suggester.SuggestionsFor("band"), bananaNode);
        }
        
        [TestMethod]
        public void suggestionsForFullQueryStringMatchAreCaseInsensitive() 
        {
            var bananaNode = new TestNode("banana");
            var bandanaNode = new TestNode("bandana");
            indexer.AddToIndex(bananaNode);
            indexer.AddToIndex(bandanaNode);

            AssertContainsSearchResult(suggester.SuggestionsFor("BAND"), bandanaNode);
            AssertContainsSearchResult(suggester.SuggestionsFor("BaNd"), bandanaNode);
        }
        
        [TestMethod]
        public void allSuggestionsForFullQueryStringMatchAreReturned() 
        {
            var bananaNode = new TestNode("banana");
            var bandanaNode = new TestNode("bandana");
            indexer.AddToIndex(bananaNode);
            indexer.AddToIndex(bandanaNode);

            AssertContainsSearchResult(suggester.SuggestionsFor("ban"), bandanaNode);
            AssertContainsSearchResult(suggester.SuggestionsFor("ban"), bananaNode);
        }

        [TestMethod]
        public void suggestionsWithSingleSplitQueryMatchAreReturned() 
        {
            var bananaPantsNode = new TestNode("bananaPants");
            indexer.AddToIndex(bananaPantsNode);

            var x = suggester.SuggestionsFor("banP").ToList();
            AssertContainsSearchResult(suggester.SuggestionsFor("banP"), bananaPantsNode);
        }
        
        [TestMethod]
        public void suggestionsWithSplitStringMatchesDoNotTranposeOrder() 
        {
            var PantsbananaNode = new TestNode("Pantsbanana");
            indexer.AddToIndex(PantsbananaNode);

            Assert.AreEqual(0, suggester.SuggestionsFor("banP").Count());
        }
        
        [TestMethod]
        public void suggestionsWithVariedLengthSingleSplitQueryMatchAreReturned() 
        {
            var bananaPantsNode = new TestNode("bananaPants");
            indexer.AddToIndex(bananaPantsNode);

            AssertContainsSearchResult(suggester.SuggestionsFor("banP"), bananaPantsNode);
            AssertContainsSearchResult(suggester.SuggestionsFor("banPa"), bananaPantsNode);
        }

        [TestMethod]
        public void suggestionsWithADoubleSplitQueryMatchAreReturned() 
        {
            var bananaPantsNode = new TestNode("bananaPants");
            indexer.AddToIndex(bananaPantsNode);

            AssertContainsSearchResult(suggester.SuggestionsFor("banPts"), bananaPantsNode);
        }

        [TestMethod]
        [Ignore]
        public void searchResultsContainsAllMatchedRanges()
        {
            var bananaPantsNode = new TestNode("bananaPants");
            indexer.AddToIndex(bananaPantsNode);

            var ranges = suggester.SuggestionsFor("banPts").First().MatchRanges;

            var expected = new List<Tuple<int, int>>
            {
                Tuple.Create(0, 3),
                Tuple.Create(6, 1),
                Tuple.Create(9, 2)
            };
            CollectionAssert.AreEqual(expected, ranges);
        }

        private void AssertContains(IEnumerable<TestNode> items, object item)
        {
            CollectionAssert.Contains(items.ToList(), item);
        }
        
        private void AssertDoesNotContain(IEnumerable<TestNode> items, object item)
        {
            CollectionAssert.DoesNotContain(items.ToList(), item);
        }
        
        private void AssertContainsSearchResult(IEnumerable<SearchResult<TestNode>> searchResults, object item)
        {
            AssertContains(searchResults.Select(sr => sr.Node), item);
        }
        
        private void AssertDoesNotContainSearchResult(IEnumerable<SearchResult<TestNode>> searchResults, object item)
        {
            AssertDoesNotContain(searchResults.Select(sr => sr.Node), item);
        }
    }
}
