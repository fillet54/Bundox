using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace Bundox.Core.Data.DocSet
{
    [Table(Name="ZTOKEN")]
    public class TokenEntity
    {
        [Column(Name = "Z_PK")]
        public int Id { get; set; }

        [Column(Name = "ZMETAINFORMATION")]
        public int MetaInformationId { get; set; }

        [Column(Name = "ZTOKENNAME")]
        public string Name { get; set; }

        [Column(Name = "ZTOKENTYPE")]
        public int Type { get; set; }
    }
}
