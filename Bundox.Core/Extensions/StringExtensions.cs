using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bundox.Core.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<List<string>> Partition(this string theString, int numPartitions)
        {
            if (numPartitions == 1)
            {
                yield return new List<string> { theString };
                yield break; 
            }

            foreach (var i in Enumerable.Range(1, theString.Length - 1))
            {
                var prefix = theString.Substring(0, theString.Length - i);
                var suffix = theString.Substring(theString.Length - i);
                foreach (var partition in prefix.Partition(numPartitions-1))
                {
                    var iteration = new List<string>(partition);
                    iteration.Add(suffix);
                    yield return iteration;
                }
            }
        }
    }
}
