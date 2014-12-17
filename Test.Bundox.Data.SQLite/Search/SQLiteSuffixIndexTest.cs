using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;
using Bundox.Data.SQLite.Search;
using Bundox.Core.Search;

namespace Test.Bundox.Data.SQLite.Search
{
    [TestClass]
    public class SQLiteSuffixIndexTest
    {
        [TestMethod]
        public void CanRetreiveEachByEachSuffix()
        {
            var node = new SuffixIndexNode { Term="SomeTerm", ID=1 };
            var indexer = new SQLiteSuffixIndexer(Path.GetTempFileName());
            indexer.InitializeDB();

            indexer.AddToIndex(node);

            Assert.AreEqual(1, indexer.Retrieve("SomeTerm").Count());
            Assert.AreEqual(1, indexer.Retrieve("omeTerm").Count());
            Assert.AreEqual(1, indexer.Retrieve("meTerm").Count());
            Assert.AreEqual(1, indexer.Retrieve("eTerm").Count());
            Assert.AreEqual(1, indexer.Retrieve("Term").Count());
            Assert.AreEqual(1, indexer.Retrieve("erm").Count());
            Assert.AreEqual(1, indexer.Retrieve("rm").Count());
            Assert.AreEqual(1, indexer.Retrieve("m").Count());
        }
        
        [TestMethod]
        public void ReturnsMatchingResults()
        {
            var node1 = new SuffixIndexNode { Term="SomeTerm", ID=1 };
            var node2 = new SuffixIndexNode { Term="AnotherTerm", ID=2 };
            var indexer = new SQLiteSuffixIndexer(Path.GetTempFileName());
            indexer.InitializeDB();

            indexer.AddToIndex(node1);
            indexer.AddToIndex(node2);

            Assert.AreEqual(1, indexer.Retrieve("eTerm").First().ID);
            Assert.AreEqual(1, indexer.Retrieve("eTerm").Count());
            Assert.AreEqual(2, indexer.Retrieve("rTerm").First().ID);
            Assert.AreEqual(1, indexer.Retrieve("rTerm").Count());
        }

        [TestMethod]
        public void ReturnsAllMatchingResults()
        {
            var node1 = new SuffixIndexNode { Term="SomeTerm", ID=1 };
            var node2 = new SuffixIndexNode { Term="AnotherTerm", ID=2 };
            var indexer = new SQLiteSuffixIndexer(Path.GetTempFileName());
            indexer.InitializeDB();

            indexer.AddToIndex(node1);
            indexer.AddToIndex(node2);

            Assert.AreEqual(2, indexer.Retrieve("Term").Count());
        }
       
        [TestMethod]
        public void ReturnsAllSubstringMatchingResults()
        {
            var node1 = new SuffixIndexNode { Term="SomeTerm", ID=1 };
            var node2 = new SuffixIndexNode { Term="AnotherTerm", ID=2 };
            var indexer = new SQLiteSuffixIndexer(Path.GetTempFileName());
            indexer.InitializeDB();

            indexer.AddToIndex(node1);
            indexer.AddToIndex(node2);

            Assert.AreEqual(2, indexer.Retrieve("Ter").Count());
        }
        
        [TestMethod]
        public void RetrieveIsCaseInsentive()
        {
            var node1 = new SuffixIndexNode { Term="SomeTerm", ID=1 };
            var node2 = new SuffixIndexNode { Term="AnotherTerm", ID=2 };
            var indexer = new SQLiteSuffixIndexer(Path.GetTempFileName());
            indexer.InitializeDB();

            indexer.AddToIndex(node1);
            indexer.AddToIndex(node2);

            Assert.AreEqual(2, indexer.Retrieve("ter").Count());
        }

        [TestMethod]
        public void CanAddInBatches()
        {
            var node1 = new SuffixIndexNode { Term="SomeTerm", ID=1 };
            var node2 = new SuffixIndexNode { Term="AnotherTerm", ID=2 };
            var indexer = new SQLiteSuffixIndexer(Path.GetTempFileName());
            indexer.InitializeDB();

            indexer.AddToIndex(node1, node2);

            Assert.AreEqual(2, indexer.Retrieve("Term").Count());
        }
    }
}
