using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bundox.Data.SQLite.DocSet;
using System.Linq;
using System.IO;

namespace Test.Bundox.Data.SQLite
{
    [TestClass]
    public class SQLiteDocSetRepositoryTest
    {
        [TestMethod]
        public void TestCanGetAllTokensFromSearchIndexBasedSQLiteDocSetDatabase()
        {
            var repo = GetSearchIndexBasedSQLiteDocSetRepository(); 

            var docSetEntities = repo.GetAll();

            Assert.AreEqual(15, docSetEntities.Count());
        }

        [TestMethod]
        public void TestCanGetAllTokensFromZTokenBasedSQLiteDocSetDatabase()
        {
            var repo = GetZTokenBasedSQLiteDocSetRepository(); 

            var docSetEntities = repo.GetAll();

            Assert.AreEqual(1360, docSetEntities.Count());
        }
        
        [TestMethod]
        public void GetsZeroTokensFromSQLiteDocSetDatabaseWhenFormatIsUnknown()
        {
            var repo = new SQLiteDocSetRepository(Path.GetTempPath()); 

            var docSetEntities = repo.GetAll();

            Assert.AreEqual(0, docSetEntities.Count());
        }

        [TestMethod]
        public void CanExtractsCorrectFieldsIntoDocSetEntityFromSearchIndexBasedSQLiteDocSetRepository()
        {
            var repo = GetSearchIndexBasedSQLiteDocSetRepository();

            var docSetEntity = repo.GetAll().First();

            Assert.AreEqual(1, docSetEntity.ID);
            Assert.AreEqual("Philosophy", docSetEntity.Name);
            Assert.AreEqual("Guide", docSetEntity.Type);
            Assert.AreEqual("index.html#philosophy", docSetEntity.Path);
        }

        [TestMethod]
        public void CanExtractsCorrectFieldsIntoDocSetEntityFromZTokenBasedSQLiteDocSetRepository()
        {
            var repo = GetZTokenBasedSQLiteDocSetRepository();

            var docSetEntity = repo.GetAll().First();

            Assert.AreEqual(1, docSetEntity.ID);
            Assert.AreEqual("plus(java.util.Map)", docSetEntity.Name);
            Assert.AreEqual("Method", docSetEntity.Type);
            Assert.AreEqual("java/util/Map.html#plus(java.util.Map)", docSetEntity.Path);
        }
        
        [TestMethod]
        public void CanGetSingleEntityByIdFromSearchIndexBasedSQLiteDocSetRepository()
        {
            var repo = GetSearchIndexBasedSQLiteDocSetRepository();

            var docSetEntity = repo.GetEntityById(1);

            Assert.AreEqual(1, docSetEntity.ID);
            Assert.AreEqual("Philosophy", docSetEntity.Name);
            Assert.AreEqual("Guide", docSetEntity.Type);
            Assert.AreEqual("index.html#philosophy", docSetEntity.Path);
        }

        [TestMethod]
        public void CanGetSingleEntityByIdFromZTokenBasedSQLiteDocSetRepository()
        {
            var repo = GetZTokenBasedSQLiteDocSetRepository();

            var docSetEntity = repo.GetEntityById(1);

            Assert.AreEqual(1, docSetEntity.ID);
            Assert.AreEqual("plus(java.util.Map)", docSetEntity.Name);
            Assert.AreEqual("Method", docSetEntity.Type);
            Assert.AreEqual("java/util/Map.html#plus(java.util.Map)", docSetEntity.Path);
        }

        private SQLiteDocSetRepository GetSearchIndexBasedSQLiteDocSetRepository()
        {
            return new SQLiteDocSetRepository(@"DocSet\Resources\markdown.docset.dsidx");
        }
        
        private SQLiteDocSetRepository GetZTokenBasedSQLiteDocSetRepository()
        {
            return new SQLiteDocSetRepository(@"DocSet\Resources\groovy.docset.dsidx");
        }

    }
}
