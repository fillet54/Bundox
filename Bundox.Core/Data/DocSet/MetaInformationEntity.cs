using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace Bundox.Core.Data.DocSet
{
    [Table(Name = "ZTOKENMETAINFORMATION")]
    public class MetaInformationEntity
    {
        [Column(Name = "Z_PK")]
        public int Id { get; set; }

        [Column(Name = "ZFILE")]
        public int FilePathId { get; set; }

        [Column(Name = "ZANCHOR")]
        public string Anchor { get; set; }
    }
}
