using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace Bundox.Core.Model.DocSet
{
    [Table(Name="ZTOKEN")]
    public class Token
    {
        [Column(Name = "Z_PK")]
        public int Id { get; set; }

        [Column(Name = "Z_ENT")]
        public int Ent { get; set; }

        [Column(Name = "Z_OPT")]
        public int Opt { get; set; }

        [Column(Name = "ZTOKENNAME")]
        public string Name { get; set; }

        [Column(Name = "ZTOKENTYPE")]
        public int Type { get; set; }
    }
}
