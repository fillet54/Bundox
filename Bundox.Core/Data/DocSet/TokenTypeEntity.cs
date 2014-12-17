using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace Bundox.Core.Data.DocSet
{
    [Table(Name = "ZTOKENTYPE")]
    public class TokenTypeEntity
    {
        [Column(Name = "Z_PK")]
        public int ID { get; set; }

        [Column(Name = "ZTYPENAME")]
        public string Name { get; set; }
    }
}
