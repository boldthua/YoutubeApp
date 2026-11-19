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
using YoutubeApp.Utilities;
using YoutubeApp.ViewModels;

namespace YoutubeApp.Pages
{
    /// <summary>
    /// ShowResultPage.xaml 的互動邏輯
    /// </summary>
    public partial class ShowResultPage : Page
    {
        public ShowResultPage(INavigationService service)
        {
            InitializeComponent();
            DataContext = new ShowResultContext(service);
        }
    }
}
