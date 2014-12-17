using Bundox.Core.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bundox.Core.Extensions;

namespace Bundox.Core.Search
{
    public class SuffixIndexNode : Indexible
    {
        public int ID { get; set; }
        public string Term { get; set; }

        public string IndexOn
        {
            get { return string.Join(" ", Term.Suffixes().ToArray()); }
            set { Term = value.Split(new char[] { ' ' }).First(); }
        }
    }
}
