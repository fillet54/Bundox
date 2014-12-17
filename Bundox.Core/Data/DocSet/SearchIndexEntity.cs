using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace Bundox.Core.Data.DocSet
{
    [Table(Name = "searchIndex")]
    public class SearchIndexEntity
    {
        [Column(Name = "id")]
        public int ID { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "type")]
        public string Type { get; set; }

        [Column(Name = "path")]
        public string Path { get; set; }
    }
}
