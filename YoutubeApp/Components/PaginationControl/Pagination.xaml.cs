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

            _context.OnPageChanged = (page) =>
            {
                if (GoToPageCommand != null && GoToPageCommand.CanExecute(page))
                {
                    GoToPageCommand.Execute(page);
                }
            };
        }

        public static readonly DependencyProperty TotalPagesProperty =
                    DependencyProperty.Register(
                        nameof(TotalPages),
                        typeof(int),
                        typeof(Pagination),
                        new PropertyMetadata(1, (d, e) =>
                        {
                            if (d is Pagination control && e.NewValue is int total)
                            {
                                control._context.Init(total);
                            }
                        }));

        public int TotalPages
        {
            get => (int)GetValue(TotalPagesProperty);
            set => SetValue(TotalPagesProperty, value);
        }

        public static readonly DependencyProperty GoToPageCommandProperty =
                    DependencyProperty.Register(
                        nameof(GoToPageCommand),
                        typeof(ICommand),
                        typeof(Pagination),
                        new PropertyMetadata(null)); // 保持 null 即可，不要覆蓋內部的 _context 指令
        public ICommand GoToPageCommand
        {
            get => (ICommand)GetValue(GoToPageCommandProperty);
            set => SetValue(GoToPageCommandProperty, value);
        }
    }
}
