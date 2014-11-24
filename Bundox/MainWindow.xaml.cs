using Bundox.Core;
using Bundox.Core.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            var suggester = new FuzzySuggester<SampleData>();
            SampleDataRepository.LoadFromDb(t => suggester.AddToIndex(new SampleData { Id = t.Id, Name = t.Name, Description = "" }));
            this.DataContext = new SampleViewModel(suggester);
        }
    }
}
