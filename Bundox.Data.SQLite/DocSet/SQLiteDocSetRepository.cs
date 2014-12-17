using Bundox.Core.Data.DocSet;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Bundox.Data.SQLite.DocSet
{
    public class SQLiteDocSetRepository : SQLiteDocSetRepositoryBase
    {
        private IDocSetRepository _InnerDocSetRepository { get; set; }

        public SQLiteDocSetRepository(string dbPath)
            :base(dbPath)
        {
            _InnerDocSetRepository = GetDocsetRepositoryForDB();
        }

        public override IEnumerable<DocSetEntity> GetAll()
        {
            return _InnerDocSetRepository.GetAll();
        }

        public override DocSetEntity GetEntityById(int id)
        {
            return _InnerDocSetRepository.GetEntityById(id);
        }

        private IDocSetRepository GetDocsetRepositoryForDB()
        {
            IDocSetRepository sqliteDB = null;
            using (var connection = new SQLiteConnection(_ConnectionString))
            {
                connection.Open();
                if (TableExists(connection, "searchIndex"))
                {
                    sqliteDB = new SearchIndexBasedSQLiteDocSetRepository(_DatabasePath);
                }
                else if (TableExists(connection, "ZTOKEN"))
                {
                    sqliteDB = new ZTokenBasedSQLiteDocSetRepository(_DatabasePath);
                }
                else
                {
                    sqliteDB = new NullSQLiteDocSetRepository();
                }
                connection.Close();
            }
            return sqliteDB;
        }

        private bool TableExists(IDbConnection connection, string tableName)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM sqlite_master WHERE name=@TableName";
            var p1 = cmd.CreateParameter();
            p1.DbType = DbType.String;
            p1.ParameterName = "TableName";
            p1.Value = tableName;
            cmd.Parameters.Add(p1);

            var result = cmd.ExecuteScalar();
            return ((long)result) == 1;
        }
    }
}
