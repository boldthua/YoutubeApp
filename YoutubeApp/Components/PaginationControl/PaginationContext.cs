using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YoutubeApp.Models;
using YoutubeApp.Utilities;

namespace YoutubeApp.Components.PaginationControl
{
    [AddINotifyPropertyChangedInterface]
    public class PaginationContext
    {
        // 這是DP
        public ICommand GoToPageCommand { get; set; }
        public int TotalPages { get; set; }

        // 以下是一般屬性
        public ICommand Go10PageFowardCommand { get; set; }
        public ICommand Go10PageBackwardCommand { get; set; }
        public ICommand GoToTargetPageCommand { get; set; }
        public ICommand GoToNumberPageCommand { get; set; }
        public string PageText { get; set; }
        public int ItemCountsInOnePage = 2;

        public int CurrentPage { get; set; } = 1;
        public bool Is10PagesFoward { get; set; } = false;
        public bool Is10PagesBackward { get; set; } = false;
        public ObservableCollection<PageItem> PageSet { get; set; } = new ObservableCollection<PageItem>();

        public PaginationContext()
        {
            Go10PageFowardCommand = new RelayCommand(() =>
            {
                int pageToGo = CurrentPage - 10;
                GoToPage(pageToGo);
            });

            Go10PageBackwardCommand = new RelayCommand(() =>
            {
                int pageToGo = Math.Min(CurrentPage + 10, TotalPages);
                GoToPage(pageToGo);
            });
            GoToTargetPageCommand = new RelayCommand(() =>
            {
                if (!int.TryParse(PageText, out int pageToGo))
                {
                    MessageBox.Show("請輸入數字！");
                    PageText = string.Empty;
                    return;
                }
                GoToPage(pageToGo);
                PageText = string.Empty;
            });
            GoToNumberPageCommand = new RelayCommand<string>((pageNum) =>
            {
                int pageToGo = int.Parse(pageNum);
                GoToPage(pageToGo);
            });

        }
        public void Init(int totalResults)
        {
            TotalPages = totalResults < 1 ? 1 : totalResults;
            GoToPage(1);
        }

        public void GoToPage(int pageToGo)
        {
            if (pageToGo > TotalPages)
            {
                return;
            }
            CurrentPage = pageToGo;

            // 當前頁數若小於等於10，則不顯示上一個10頁按鈕
            Is10PagesFoward = CurrentPage <= 10 ? false : true;

            // 當前頁數若和最大頁數在同一個10頁區間，則不顯示下一個10頁按鈕
            Is10PagesBackward = (CurrentPage - 1) / 10 == TotalPages / 10 ? false : true;

            int firstPageNum = ((pageToGo - 1) / 10) * 10 + 1;
            PageSet.Clear();
            int lastPageNum = Math.Min(firstPageNum + 9, TotalPages);
            for (int i = firstPageNum; i <= lastPageNum; i++)
            {
                string thickness = i == CurrentPage ? "4" : "2";
                PageSet.Add(new PageItem(i.ToString(), thickness));
            }

            GoToPageCommand?.Execute(pageToGo);
        }

        public void TenPagesForward()
        {
            CurrentPage = CurrentPage / 10 * 10 - 9;
            GoToPage(CurrentPage);
        }

        public void TenPagesBackward()
        {
            CurrentPage = CurrentPage / 10 * 10 + 11;
            GoToPage(CurrentPage);
        }

        [AddINotifyPropertyChangedInterface]
        public class PageItem
        {
            public string Text { get; set; }
            public string Thickness { get; set; }

            public PageItem(string text, string thickness)
            {
                Text = text;
                Thickness = thickness;
            }
        }
    }
}
