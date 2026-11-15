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
using YoutubeApp.Components.SearchFilterControl;

namespace YoutubeApp.Components.PaginationControl
{
    /// <summary>
    /// Pagination.xaml 的互動邏輯
    /// </summary>
    public partial class Pagination : UserControl
    {
        private readonly PaginationContext _context;
        public Pagination()
        {
            InitializeComponent();
            _context = new PaginationContext();
            paginationComponent.DataContext = _context;
            var mainWindow = Application.Current.MainWindow;
        }

        public static readonly DependencyProperty TotalPagesProperty =
                    DependencyProperty.Register(
                        nameof(TotalCount),
                        typeof(int),
                        typeof(Pagination),
                        new PropertyMetadata(1, (d, e) =>
                        {
                            if (d is Pagination control && e.NewValue is int total)
                            {
                                control._context.Init(total);
                            }
                        }));

        public int TotalCount
        {
            get => (int)GetValue(TotalPagesProperty);
            set => SetValue(TotalPagesProperty, value);
        }

        public static readonly DependencyProperty GoToPageCommandProperty =
                    DependencyProperty.Register(
                        nameof(GoToPageCommand),
                        typeof(ICommand),
                        typeof(Pagination),
                        new PropertyMetadata((d, e) =>
                        {
                            Pagination control = d as Pagination;
                            PaginationContext context = control.paginationComponent.DataContext as PaginationContext;
                            context.GoToPageCommand = e.NewValue as ICommand;
                        }));
        public ICommand GoToPageCommand
        {
            get => (ICommand)GetValue(GoToPageCommandProperty);
            set => SetValue(GoToPageCommandProperty, value);
        }

        private void PageTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }
    }
}
