using Bundox.Core.Data.DocSet;
using Bundox.Core.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Test.Bundox.Core.DocSet
{
    [TestClass]
    public class DocSetIndexerTest
    {
        [TestMethod]
        public void ReturnsEmptyResultsIfIndexerHasEmptyResults()
        {
            var docSetRepo = new Mock<IDocSetRepository>();
            var suffixIndex = new Mock<ISuffixIndex<SuffixIndexNode>>();
            suffixIndex.Setup(m => m.Retrieve("TheQuery")).Returns(Enumerable.Empty<SuffixIndexNode>());
            var indexer = new DocSetIndex(docSetRepo.Object, suffixIndex.Object);

            Assert.AreEqual(0, indexer.Retrieve("TheQuery").Count());
        }

        // This test case is most likely just to help with TDD.
        // It's probably safe to assume that the SuffixIndex was built from
        // the docset repo so its not likely for there to be out of sync Ids 
        // between the two. Just to be on the safe side we'll return empty anyways.
        [TestMethod]
        public void ReturnsEmptyResultsIfDocSetRepositoryDoesNotContainEntity()
        {
            var docSetRepo = new Mock<IDocSetRepository>();
            var suffixIndex = new Mock<ISuffixIndex<SuffixIndexNode>>();
            suffixIndex.Setup(m => m.Retrieve("TheQuery")).Returns(Enumerable.Empty<SuffixIndexNode>());
            var indexer = new DocSetIndex(docSetRepo.Object, suffixIndex.Object);

            Assert.AreEqual(0, indexer.Retrieve("TheQuery").Count());
        }

        [TestMethod]
        public void ReturnsTheMatchingDocSetEntity()
        {
            var entity = new DocSetEntity();
            var docSetRepo = new Mock<IDocSetRepository>();
            docSetRepo.Setup(m => m.GetEntityById(1)).Returns(entity);
            var suffixIndex = new Mock<ISuffixIndex<SuffixIndexNode>>();
            suffixIndex.Setup(m => m.Retrieve("TheQuery")).Returns(new List<SuffixIndexNode> { new SuffixIndexNode { ID = 1 } });
            var indexer = new DocSetIndex(docSetRepo.Object, suffixIndex.Object);

            Assert.AreEqual(1, indexer.Retrieve("TheQuery").Count());
            Assert.AreEqual(1, indexer.Retrieve("TheQuery").First().ID);
        }
    }
}
