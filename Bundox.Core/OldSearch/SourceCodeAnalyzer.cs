using Lucene.Net.Analysis;
using Lucene.Net.Analysis.NGram;
using Lucene.Net.Analysis.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bundox.Core.Search
{
    public class SourceCodeAnalyzer : Analyzer
    {
        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            var tokenStream = new LowerCaseTokenizer(reader);

            return new NGramTokenFilter(tokenStream, 1, 4);
        }
    }
}
