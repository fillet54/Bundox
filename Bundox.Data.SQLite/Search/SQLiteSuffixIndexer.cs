using Bundox.Core.Data;
using Bundox.Core.Search;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Bundox.Data.SQLite.Search
{
    public class SQLiteSuffixIndexer : ISuffixIndexer<SuffixIndexNode>
    {
        private string _DBFilePath;

        public SQLiteSuffixIndexer(string dbFilePath)
        {
            _DBFilePath = dbFilePath;
        }

        public void InitializeDB()
        {
            SQLiteConnection.CreateFile(_DBFilePath);
            using (var connection = new SQLiteConnection(string.Format("Data Source={0};Version=3", _DBFilePath)))
            {
                connection.Open();
                var SQL = "CREATE VIRTUAL TABLE searchIndex USING FTS4 ( id, suffixes )";

                using (var cmd = new SQLiteCommand(SQL, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void AddToIndex(params SuffixIndexNode[] items)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var SQL = "INSERT INTO searchIndex(id, suffixes) VALUES (@ID,@Suffixes)";
                    using (var cmd = new SQLiteCommand(SQL, connection))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@ID"));
                        cmd.Parameters.Add(new SQLiteParameter("@Suffixes"));
                        items.ToList().ForEach(item =>
                        {
                            cmd.Parameters["@ID"].Value =  item.ID;
                            cmd.Parameters["@Suffixes"].Value =  item.IndexOn;
                            cmd.ExecuteNonQuery();
                        });
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
        }

        public IEnumerable<SuffixIndexNode> Retrieve(params string[] substrings)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                var SQL = "SELECT * FROM searchIndex WHERE suffixes MATCH @Query";

                using (var cmd = new SQLiteCommand(SQL, connection))
                {
                    cmd.Parameters.AddWithValue("@Query", string.Join("*", substrings) + "*");
                    cmd.CommandType = System.Data.CommandType.Text;
                    var reader = cmd.ExecuteReader();

                    while(reader.Read())
                        yield return new SuffixIndexNode { ID = Convert.ToInt32(reader["id"]), IndexOn = Convert.ToString(reader["suffixes"])};
                }
                connection.Close();
            }
        }

        private SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(string.Format("Data Source={0};Version=3", _DBFilePath));
        }
    }
}
