using Bundox.Core.Data.DocSet;
using Bundox.Core.Search;
using Bundox.ViewModels;
using Bundox.Wpf.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bundox
{
    public class SampleViewModel : ViewModelBase
    {
        private string _PageSource;
        public string PageSource
        {
            get { return _PageSource; }
            set
            {
                if (_PageSource != value)
                {
                    _PageSource = value;
                    RaisePropertyChanged("PageSource");
                }
            }
        }

        private List<SearchResultViewModel> _SearchResults = new List<SearchResultViewModel>();
        public List<SearchResultViewModel> SearchResults 
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

        private SearchResultViewModel _SelectedResult;
        public SearchResultViewModel SelectedResult
        {
            get { return _SelectedResult; }
            set
            {
                if (_SelectedResult != value)
                {
                    _SelectedResult = value;
                    RaisePropertyChanged("SelectedResult");
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
                       .Take(50);

            SearchResults = temp
                            .Select(sr => new SearchResultViewModel
                            {
                                HighlightedName = HighlightMatchedSubstrings(sr.Node.IndexOn, sr.MatchRanges),
                                Entity = sr.Node
                            })
                            .ToList();
            var end = DateTime.UtcNow;
            Console.WriteLine("Measured time: " + (end - start).TotalMilliseconds + "ms");
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

        private ISuggester<DocSetEntity> Suggester { get; set; }

        public SampleViewModel(ISuggester<DocSetEntity> suggester)
        {
            Suggester = suggester;

            var pathBase = @"C:\Projects\Docsets\Java.docset\Contents\Resources\Documents\";
            this.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "SearchQuery")
                    {
                        OnSearchQueryUpdate(SearchQuery);
                    }
                    else if (e.PropertyName == "SelectedResult")
                    {
                        if (SelectedResult != null)
                        {
                            var pageSource = pathBase + SelectedResult.Entity.Path;
                            PageSource = pageSource;
                        }
                    }
                };

            PageSource = @"C:\Projects\Docsets\Java.docset\Contents\Resources\Documents\overview-summary.html";
        }
    }
}
