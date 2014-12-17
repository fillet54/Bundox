using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace Bundox.Core.Data.DocSet
{
    [Table(Name="ZFILEPATH")]
    public class FilePathEntity
    {
        [Column(Name = "Z_PK")]
        public int Id { get; set; }

        [Column(Name = "ZPATH")]
        public string Path { get; set; }
    }
}
