using AutoMapper;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YoutubeAPI;
using YoutubeAPI.Models;
using YoutubeApp.Models;
using YoutubeApp.Utilities;
using static YoutubeApp.Contracts.VideoSearchContract;

namespace YoutubeApp.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class ShowResultContext : IVedioSearchView
    {
        public ObservableCollection<SubscriptionList.Item> myFavorites { get; set; } = new ObservableCollection<SubscriptionList.Item>();
        IVideoSearchPresenter presenter { get; set; }
        public string _searchKeyword; // 需要DP
        public string nextPageToken { get; set; }
        public int TotalCount { get; set; }
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
        public ChannelsModel.Item myAccount { get; set; }
        public ShowResultContext(INavigationService service)
        {
            // 一開始就要戴入 ShowResultPage
            service.Navigate("ShowResultPage");

            presenter = new VideoSearchPresenter(this);
            presenter.GetSubscriptionLists();
            moreSubscribCommand = new RelayCommand(GetMoreSubcrib);
            searchFilterCommand = new RelayCommand<Dictionary<string, string>>(SearchFilter);
            searchCommand = new RelayCommand(async () =>
            {
                parm = new Dictionary<string, string>();
                await presenter.StartSearch(_searchKeyword);
            });
            GoToPageCommand = new RelayCommand<int>(GoToPage);
        }



        private void SetSearchResult(IEnumerable<object> items)
        {
            AllSearchResultItems = items.ToList();

            // 總頁數無條件進位（例如 5 筆資料，每頁 2 筆，應該要有 3 頁）
            TotalCount = (int)Math.Ceiling((double)AllSearchResultItems.Count / ItemCountsInOnePage);

            // 直接取第一頁的數量顯示
            CurrentShownItems = new ObservableCollection<object>(
                AllSearchResultItems.Take(ItemCountsInOnePage));
        }

        public string SearchKeyWord
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                IsSearchKeyWordExist = !string.IsNullOrWhiteSpace(value);
            }
        }

        public void SubscriptionItemsResponse(SubscriptionList.Item[] items, string nextPageToken)
        {
            foreach (SubscriptionList.Item item in items)
            {
                myFavorites.Add(item);
            }
            this.nextPageToken = nextPageToken;
            if (!string.IsNullOrEmpty(nextPageToken))
                IsMoreSubscrib = true;
            else
                IsMoreSubscrib = false;
        }
        public void GetMoreSubcrib()
        {
            presenter.GetSubscriptionLists(nextPageToken);
        }

        public void MyAccountResponse(ChannelsModel.Item myAccount)
        {
            this.myAccount = myAccount;
        }

        public void videoInfoResponse(List<YouTubeVideoViewModel> videoInfos)
        {
            SetSearchResult(videoInfos);
        }

        public void ChannelInfoResponse(List<ChannelsModel.Item> items)
        {
            Mapper mapper = new Mapper();
            var mappedChannels = items.Select(x => mapper.Map<ChannelModelItem, ChannelsModel.Item>(x));
            SetSearchResult(mappedChannels);
        }

        public void PlayListInfoResponse(PlayListItemsList playListItemsList)
        {
            Mapper mapper = new Mapper();
            var mappedPlaylists = playListItemsList.items.Select(x => mapper.Map<PlayListItem, PlayListItemsList.Item>(x));
            SetSearchResult(mappedPlaylists);
        }

        public void SearchFilter(Dictionary<string, string> parm)
        {
            this.parm = parm;
            presenter.StartSearch(_searchKeyword, parm);
        }

        public void GoToPage(int pageToGo)
        {
            if (pageToGo < 1 || pageToGo > TotalCount)
            {
                MessageBox.Show("沒有這麼多頁！");
                return;
            }

            var pagedItems = AllSearchResultItems
                .Skip((pageToGo - 1) * ItemCountsInOnePage)
                .Take(ItemCountsInOnePage);

            CurrentShownItems = new ObservableCollection<object>(pagedItems);
        }
    }
}
