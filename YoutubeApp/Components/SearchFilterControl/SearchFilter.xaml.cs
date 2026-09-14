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


namespace YoutubeApp.Components.SearchFilterControl
{
    /// <summary>
    /// SearchFilter.xaml 的互動邏輯
    /// </summary>
    public partial class SearchFilter : UserControl          // xaml的codeBehind，和xaml是一體的
    {

        public SearchFilter()
        {
            InitializeComponent();
            SearchFilterContext context = new SearchFilterContext();
            searchFilterComponent.DataContext = context;
            TextBox textBox = new TextBox();
            textBox.Text = string.Empty;
        }

        public static readonly DependencyProperty MyUserValueProperty = DependencyProperty.Register(
            nameof(SearchFilterCommand),
            typeof(ICommand),
            typeof(SearchFilter),
            new PropertyMetadata((d, e) =>
            {
                SearchFilter search = d as SearchFilter;
                SearchFilterContext context = search.searchFilterComponent.DataContext as SearchFilterContext;
                context.SearchCommand = e.NewValue as ICommand;
            }));
        public ICommand SearchFilterCommand
        {
            get => (ICommand)GetValue(MyUserValueProperty);
            set => SetValue(MyUserValueProperty, value);
        }


    }
}
