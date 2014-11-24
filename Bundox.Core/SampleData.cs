using Bundox.Core.Model.DocSet;
using Bundox.Core.Search;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bundox.Core
{
    public class SampleData : Indexible
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string IndexOn
        {
            get { return Name; }
        }
    }

    public static class SampleDataRepository
    {
        public static SampleData Get(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id.Equals(id));
        }

        public static void LoadFromDb(Action<Token> loadToken)
        {
            var connection = new SQLiteConnection(@"Data Source=java.sqlite;Version=3");
            var context = new DataContext(connection);

            context.GetTable<Token>()
                   //.Where(t => t.Name.StartsWith("a"))
                   .ToList()
                   .ForEach(loadToken);
            connection.Close();
        }

        public static List<SampleData> GetAll()
        {
            
            return new List<SampleData> {
                new SampleData {Id = 1, Name = "Belgrad", Description = "City in Serbia"},
                new SampleData {Id = 2, Name = "Moscow", Description = "City in Russia"},
                new SampleData {Id = 3, Name = "Chicago", Description = "City in USA"},
                new SampleData {Id = 4, Name = "Mumbai", Description = "City in India"},
                new SampleData {Id = 5, Name = "Hong-Kong", Description = "City in Hong-Kong"},
                
                new SampleData {Id = 6, Name = "Moscows", Description = "City in Russia"},
                new SampleData {Id = 6, Name = "Moscowality", Description = "City in Russia"},
                new SampleData {Id = 6, Name = "Moscowality", Description = "City in Russia"},
                new SampleData {Id = 6, Name = "Moscowjasdality", Description = "City in Russia"},
                new SampleData {Id = 6, Name = "Moscowalasdfty", Description = "City in Russia"},
                new SampleData {Id = 6, Name = "Moscowalitsafsy", Description = "City in Russia"},
            };
        }
    }
}
