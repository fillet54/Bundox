using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bundox.Core.Search
{
    public class FuzzyTokenizer
    {
        public static IEnumerable<string> TokensFor(string value)
        {
            var results = new List<string> { value.First().ToString() }; 
            if (value.Length == 1)
            {
                return results;
            }
            var subValues = TokensFor(value.Substring(1)).ToList();
            subValues.ForEach(s =>
            { 
                results.Add(s);
                results.Add(value.First() + s);
            });
            return results;
        }
    }
}
