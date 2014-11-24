using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var LINE_WIDTH = 80;
            var tests = new List<Tuple<string, Func<bool>>>
            {
               Tuple.Create("Returns single character", (Func<bool>)returns_single_character),
               Tuple.Create("Returns three for string of length two", (Func<bool>)returns_three_for_two_character),
               Tuple.Create("Returns seven for string of length three", (Func<bool>)returns_more_for_three_characters)
            };

            Console.WriteLine(new string('-', LINE_WIDTH));
            var failCount = 0;
            tests.ForEach(t =>
            {   var result = "PASSED";
                if (!t.Item2())
                {
                    result = "FAILED";
                    failCount++;
                }
                Console.WriteLine(string.Format("{0}{1}{2}", t.Item1, new string('.', LINE_WIDTH - 6 - t.Item1.Length), result));
            });

            Console.WriteLine(new string('-', LINE_WIDTH));
            if (failCount == 0)
            {
                Console.WriteLine("SUCCESS");
            }
            else
            {
                Console.WriteLine("ERROR");
            }
            Console.WriteLine(new string('-', LINE_WIDTH));
        }

        public static List<string> LoadFuzzySearch(string value)
        {
            var results = new List<string> { value.First().ToString() }; 
            if (value.Length == 1)
            {
                return results;
            }
            var subValues = LoadFuzzySearch(value.Substring(1));
            subValues.ForEach(s =>
            { 
                results.Add(s);
                results.Add(value.First() + s);
            });
            return results;
        }

        public static bool returns_single_character()
        {
            var expected = new List<string> { "a" };
            return ContainsAllItems(LoadFuzzySearch("a"), expected);
        }
        
        public static bool returns_three_for_two_character()
        {
            var expected = new List<string> { "a", "b", "ab" };
            return ContainsAllItems(LoadFuzzySearch("ab"), expected);
        }
        
        public static bool returns_more_for_three_characters()
        {
            var expected = new List<string> { "a", "b", "c", "ab", "ac", "bc", "abc"};
            return ContainsAllItems(LoadFuzzySearch("abc"), expected);
        }

        public static bool ContainsAllItems<T>(List<T> a, List<T> b)
        {
            return !b.Except(a).Any();
        }
    }
}
