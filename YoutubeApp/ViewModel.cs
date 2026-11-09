using AutoMapper;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeAPI;
using YoutubeAPI.Models;
using YoutubeAPI.PlayLists;
using YoutubeAPI.Videos;
using YoutubeApp.Models;
using YoutubeApp.Utilities;
using static YoutubeApp.Contracts.MainContract;

namespace YoutubeApp
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel : IMainView
    {
        public ObservableCollection<SubscriptionList.Item> myFavorites { get; set; } = new ObservableCollection<SubscriptionList.Item>();
        IMainPresenter presenter { get; set; }
        public string nextPageToken { get; set; }
        public string _searchKeyword;
        public int MaxPage { get; set; }
        public Dictionary<string, string> parm { get; set; }
        public ObservableCollection<YouTubeVideoViewModel> videoInfoItems { get; set; } = new ObservableCollection<YouTubeVideoViewModel>();
        public ObservableCollection<ChannelModelItem> ChannelInfoItems { get; set; } = new ObservableCollection<ChannelModelItem>();
        public ObservableCollection<PlayListItem> PlayListInfoItems { get; set; } = new ObservableCollection<PlayListItem>();
        //以下是裝顯示出來的單位(顯示2個而已)
        public ObservableCollection<YouTubeVideoViewModel> CurrentShownVideos { get; set; } = new ObservableCollection<YouTubeVideoViewModel>();
        public ObservableCollection<ChannelModelItem> CurrentShownChannels { get; set; } = new ObservableCollection<ChannelModelItem>();
        public ObservableCollection<PlayListItem> CurrentShownPlayLists { get; set; } = new ObservableCollection<PlayListItem>();
        public ObservableCollection<object> CurrentShownItems { get; set; } = new ObservableCollection<object>();

        public bool IsMoreSubscrib { get; set; } = false;
        public bool IsSearchKeyWordExist { get; set; }
        [DependsOn(nameof(CurrentShownVideos))]
        public bool IsResultExist => CurrentShownItems.Any();
        public ICommand moreSubscribCommand { get; set; }
        public ICommand searchCommand { get; set; }
        public ICommand searchFilterCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }
        public ICommand Go10PageFowardCommand { get; set; }
        public ICommand Go10PageBackwardCommand { get; set; }
        public ICommand GoToTargetPageCommand { get; set; }
        public string PageText { get; set; }
        public int ItemCountsInOnePage = 2;

        public ChannelsModel.Item myAccount { get; set; }

        public ViewModel()
        {
            presenter = new MainPresenter(this);
            presenter.GetSubscriptionLists();
            moreSubscribCommand = new RelayCommand(GetMoreSubcrib);
            searchFilterCommand = new RelayCommand<Dictionary<string, string>>(SearchFilter);
            searchCommand = new RelayCommand(async () =>
            {
                parm = new Dictionary<string, string>();
                await presenter.StartSearch(_searchKeyword);
            });

            presenter.GetMyAccountInfo();

            //
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
            videoInfoItems = new ObservableCollection<YouTubeVideoViewModel>(videoInfos);

            MaxPage = videoInfoItems.Count / ItemCountsInOnePage;

            List<YouTubeVideoViewModel> temVideos = new List<YouTubeVideoViewModel>();
            for (int i = 0; i < ItemCountsInOnePage; i++)
            {
                temVideos.Add(videoInfoItems[i]);
            }
            CurrentShownVideos = new ObservableCollection<YouTubeVideoViewModel>(temVideos);
            CurrentShownItems = new ObservableCollection<object>(CurrentShownVideos);
        }

        public void SearchFilter(Dictionary<string, string> parm)
        {
            this.parm = parm;
            presenter.StartSearch(_searchKeyword, parm);
        }

        public void GoToPage(int pageToGo)
        {
            if (pageToGo > pageModel.MaxPage)
            {
                MessageBox.Show("沒有這麼多頁！");
                return;
            }
            pageModel.GoToPage(pageToGo);

            if (CurrentShownItems.FirstOrDefault() is YouTubeVideoViewModel)
            {
                List<YouTubeVideoViewModel> temVideos = new List<YouTubeVideoViewModel>();

                for (int i = 0; i < ItemCountsInOnePage; i++)
                {
                    temVideos.Add(videoInfoItems[(pageToGo - 1) * ItemCountsInOnePage + i]);
                }
                CurrentShownVideos = new ObservableCollection<YouTubeVideoViewModel>(temVideos);
                CurrentShownItems = new ObservableCollection<object>(CurrentShownVideos);
            }
            else if (CurrentShownItems.FirstOrDefault() is ChannelModelItem)
            {
                List<ChannelModelItem> temChannels = new List<ChannelModelItem>();

                for (int i = 0; i < ItemCountsInOnePage; i++)
                {
                    temChannels.Add(ChannelInfoItems[(pageToGo - 1) * ItemCountsInOnePage + i]);
                }
                CurrentShownChannels = new ObservableCollection<ChannelModelItem>(temChannels);
                CurrentShownItems = new ObservableCollection<object>(CurrentShownChannels);
            }
            else if (CurrentShownItems.FirstOrDefault() is PlayListItem)
            {
                List<PlayListItem> temPlayLists = new List<PlayListItem>();

                for (int i = 0; i < ItemCountsInOnePage; i++)
                {
                    temPlayLists.Add(PlayListInfoItems[(pageToGo - 1) * ItemCountsInOnePage + i]);
                }
                CurrentShownPlayLists = new ObservableCollection<PlayListItem>(temPlayLists);
                CurrentShownItems = new ObservableCollection<object>(CurrentShownPlayLists);
            }
        }


        public void ChannelInfoResponse(List<ChannelsModel.Item> items)
        {
            Mapper mapper = new Mapper();
            ChannelInfoItems = new ObservableCollection<ChannelModelItem>(
                                   items.Select(x => mapper.Map<ChannelModelItem, ChannelsModel.Item>(x)));
            MaxPage = videoInfoItems.Count / ItemCountsInOnePage;
            CurrentShownItems = new ObservableCollection<object>(ChannelInfoItems);

            List<ChannelModelItem> temChannels = new List<ChannelModelItem>();
            for (int i = 0; i < ItemCountsInOnePage; i++)
            {
                temChannels.Add(ChannelInfoItems[i]);
            }
            CurrentShownChannels = new ObservableCollection<ChannelModelItem>(temChannels);
            CurrentShownItems = new ObservableCollection<object>(CurrentShownChannels);
        }

        public void PlayListInfoResponse(PlayListItemsList playListItemsList)
        {
            Mapper mapper = new Mapper();
            PlayListInfoItems = new ObservableCollection<PlayListItem>(
                                    playListItemsList.items.Select(x => mapper.Map<PlayListItem, PlayListItemsList.Item>(x)));
            MaxPage = videoInfoItems.Count / ItemCountsInOnePage;
            CurrentShownItems = new ObservableCollection<object>(PlayListInfoItems);

            List<PlayListItem> temPlayLists = new List<PlayListItem>();
            for (int i = 0; i < ItemCountsInOnePage; i++)
            {
                temPlayLists.Add(PlayListInfoItems[i]);
            }
            CurrentShownPlayLists = new ObservableCollection<PlayListItem>(temPlayLists);
            CurrentShownItems = new ObservableCollection<object>(CurrentShownPlayLists);
        }
    }
}
