using Bundox.Core;
using Bundox.Core.Search;
using Bundox.Wpf.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bundox
{
    public class SampleViewModel : ViewModelBase
    {
        private List<string> _SearchResults = new List<string>();
        public List<string> SearchResults 
        {
            get { return _SearchResults; }
            private set
            {
                if (_SearchResults != value)
                {
                    _SearchResults = value;
                    RaisePropertyChanged("SearchResults");
                }
            }
        }

        private String _SearchQuery = "";
        public String SearchQuery
        {
            get { return _SearchQuery; }
            set
            {
                if (_SearchQuery != value)
                {
                    _SearchQuery = value;
                    RaisePropertyChanged("SearchQuery");
                }
            }
        }

        private void OnSearchQueryUpdate(string searchQuery)
        {
            var start = DateTime.UtcNow;
            var temp = Suggester.SuggestionsFor(searchQuery)
                       .Select(sr => HighlightMatchedSubstrings(sr.Node.IndexOn, sr.MatchRanges))
                       .Take(50)
                       .ToList();
            var end = DateTime.UtcNow;
            Console.WriteLine("Measured time: " + (end - start).TotalMilliseconds + "ms");

            SearchResults = temp;

        }

        private string HIGHLIGHT_START = "|~S~|";
        private string HIGHLIGHT_END = "|~E~|";
        private string HighlightMatchedSubstrings(string subject, List<Tuple<int, int>> ranges)
        {
            var lastLocation = 0;
            var highlighted = new StringBuilder();
            foreach (var range in ranges)
            {
                highlighted.Append(subject.Substring(lastLocation, range.Item1 - lastLocation));
                highlighted.Append(HIGHLIGHT_START);
                highlighted.Append(subject.Substring(range.Item1, range.Item2));
                highlighted.Append(HIGHLIGHT_END);
                lastLocation = range.Item1 + range.Item2;
            }
            highlighted.Append(subject.Substring(lastLocation, subject.Length - lastLocation));
            return highlighted.ToString();
        }

        private ISuggester<SampleData> Suggester { get; set; }

        public SampleViewModel(ISuggester<SampleData> suggester)
        {
            Suggester = suggester;

            this.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "SearchQuery")
                    {
                        OnSearchQueryUpdate(SearchQuery);
                    }
                };
        }
    }
}
