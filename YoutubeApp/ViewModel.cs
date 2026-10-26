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
using YoutubeAPI.Models;
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
        public Dictionary<string, string> parm { get; set; }
        public ObservableCollection<YouTubeVideoViewModel> videoInfoItems { get; set; } = new ObservableCollection<YouTubeVideoViewModel>();

        public ObservableCollection<YouTubeVideoViewModel> CurrentShownVideos { get; set; } = new ObservableCollection<YouTubeVideoViewModel>();

        public bool IsMoreSubscrib { get; set; } = false;
        public bool IsSearchKeyWordExist { get; set; }
        [DependsOn(nameof(CurrentShownVideos))]
        public bool IsResultExist => CurrentShownVideos.Any();
        public ICommand moreSubscribCommand { get; set; }
        public ICommand searchCommand { get; set; }
        public ICommand searchFilterCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }
        public ICommand Go10PageFowardCommand { get; set; }
        public ICommand Go10PageBackwardCommand { get; set; }
        public ICommand GoToTargetPageCommand { get; set; }
        public PaginationModel pageModel { get; set; } = new PaginationModel();
        public string PageText { get; set; }
        public int VideoCountsInOnePage = 2;
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
            GoToPageCommand = new RelayCommand<PageItem>(ExecuteGoToPage);
            Go10PageFowardCommand = new RelayCommand(() =>
            {
                int pageToGo = pageModel.CurrentPage - 10;
                pageModel.GoToPage(pageToGo);
                GoToPage(pageToGo);
            });

            Go10PageBackwardCommand = new RelayCommand(() =>
            {
                int pageToGo = Math.Min(pageModel.CurrentPage + 10, pageModel.MaxPage);
                pageModel.GoToPage(pageToGo);
                GoToPage(pageToGo);
            });
            GoToTargetPageCommand = new RelayCommand(() =>
            {
                if(!int.TryParse(PageText, out int pageToGo))
                {
                    MessageBox.Show("請輸入數字！");
                    PageText = string.Empty;
                    return;
                }
                GoToPage(pageToGo);
                PageText = string.Empty;
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
            pageModel = new PaginationModel(videoInfoItems.Count / VideoCountsInOnePage);

            List<YouTubeVideoViewModel> temVideos = new List<YouTubeVideoViewModel>();
            for (int i = 0; i < VideoCountsInOnePage; i++)
            {
                temVideos.Add(videoInfoItems[i]);
            }
            CurrentShownVideos = new ObservableCollection<YouTubeVideoViewModel>(temVideos);

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
            List<YouTubeVideoViewModel> temVideos = new List<YouTubeVideoViewModel>();
            
            for (int i = 0; i < VideoCountsInOnePage; i++)
            {
                temVideos.Add(videoInfoItems[(pageToGo - 1) * VideoCountsInOnePage + i]);
            }
            CurrentShownVideos = new ObservableCollection<YouTubeVideoViewModel>(temVideos);
        }
        private void ExecuteGoToPage(PageItem item)
        {
            if (int.TryParse(item.Text, out int page))
            {
                if (page < 1)
                    return;

                GoToPage(page);
            }
        }

    }
}
