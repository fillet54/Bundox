using Bundox.Core;
using Bundox.Core.Data.DocSet;
using Bundox.Core.Search;
using Bundox.Data.SQLite.DocSet;
using Bundox.Data.SQLite.Search;
using System.Windows;

namespace Bundox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var docSetPath = @"C:\Projects\Docsets\Java.docset\Contents\Resources\docset.dsidx";
            var indexPath = @"C:\Projects\Docsets\Java.docset\Contents\Resources\suffixindex.dsidx";

            var repository = new SQLiteDocSetRepository(docSetPath);
            var indexer = new SQLiteSuffixIndexer(indexPath);
            var docSetIndex = new DocSetIndex(repository, indexer);

            var suggester = new FuzzySuggester<DocSetEntity>(docSetIndex);
            this.DataContext = new SampleViewModel(suggester);
        }
    }
}
