using AutoMapper;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubeAPI.Models;
using YoutubeApp.Utilities;

namespace YoutubeApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class ShowResultContext
    {
        public ObservableCollection<SubscriptionList.Item> myFavorites { get; set; } = new ObservableCollection<SubscriptionList.Item>();
        public string _searchKeyword; // 需要DP
        public Dictionary<string, string> parm { get; set; }
        // 【精簡後】只需要這 2 個集合：一個存全部結果供換頁用，一個綁定給畫面的 ItemsControl 顯示
        public List<object> AllSearchResultItems { get; set; } = new List<object>();
        public ObservableCollection<object> CurrentShownItems { get; set; } = new ObservableCollection<object>();


        public bool IsMoreSubscrib { get; set; } = false;
        public bool IsSearchKeyWordExist { get; set; }
        [DependsOn(nameof(CurrentShownItems))]
        public bool IsResultExist => CurrentShownItems.Any();
        public ICommand moreSubscribCommand { get; set; }
        public ICommand searchCommand { get; set; }
        public ICommand searchFilterCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }

        public string PageText { get; set; }
        public int ItemCountsInOnePage = 2;
        public ShowResultContext(INavigationService service)
        {

        }
    }
}
