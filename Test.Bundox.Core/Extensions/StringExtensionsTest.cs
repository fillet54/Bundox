using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Bundox.Core.Extensions;
using System.Collections.Generic;

namespace Test.Bundox.Core.Extensions
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void onePartitionsYieldsTheStringInASingleList()
        {
            var results = "Original".Partition(numPartitions: 1);

            CollectionAssert.AreEquivalent(new List<string> { "Original" }, results.First());
        }

        [TestMethod]
        public void twoParitionsReturnsASetOf8()
        {
            var results = "Original".Partition(numPartitions: 2);

            var expected = new List<List<string>> { 
                new List<string>{ "Origina", "l"}, 
                new List<string>{ "Origin", "al"},
                new List<string>{ "Origi", "nal"},
                new List<string>{ "Orig", "inal"},
                new List<string>{ "Ori", "ginal"},
                new List<string>{ "Or", "iginal"},
                new List<string>{ "O", "riginal"}
            };
            var x = results.ToList();
            CollectionAssert.Equals(expected, results.ToList());
        }
        
        [TestMethod]
        public void threeParitionsReturnsASetOf21()
        {
            var results = "Original".Partition(numPartitions: 3);

            var expected = new List<List<string>> { 
                new List<string>{ "Origin", "a", "l"}, 
                new List<string>{ "Origi", "na", "l"},
                new List<string>{ "Orig", "ina", "l"},
                new List<string>{ "Ori", "gina", "l"},
                new List<string>{ "Or", "igina", "l"},
                new List<string>{ "O", "rigina", "l"},
                
                new List<string>{ "Origi", "n", "al"},
                new List<string>{ "Orig", "in", "al"},
                new List<string>{ "Ori", "gin", "al"},
                new List<string>{ "Or", "igin", "al"},
                new List<string>{ "O", "rigin", "al"},
                
                new List<string>{ "Orig", "i", "nal"},
                new List<string>{ "Ori", "gi", "nal"},
                new List<string>{ "Or", "igi", "nal"},
                new List<string>{ "O", "rigi", "nal"},
                
                new List<string>{ "Ori", "g", "inal"},
                new List<string>{ "Or", "ig", "inal"},
                new List<string>{ "O", "rig", "inal"},

                new List<string>{ "Or", "i", "ginal"},
                new List<string>{ "O", "ri", "ginal"},

                new List<string>{ "O", "r", "iginal"}
            };
            CollectionAssert.Equals(expected, results.ToList());
        }
    }
}
