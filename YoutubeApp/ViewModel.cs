using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public bool IsMoreSubscrib { get; set; } = false;
        public bool IsSearchKeyWordExist { get; set; }
        public ICommand moreSubscribCommand { get; set; }
        public ICommand searchCommand { get; set; }
        public ICommand searchFilterCommand { get; set; }
        public ICommand pageSearchCommand { get; set; }
        public PaginationModel pageModel { get; set; }

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
            pageSearchCommand = new RelayCommand<string>(SearchPage);
            presenter.GetMyAccountInfo();

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

        public void videoInfoResponse(List<YouTubeVideoViewModel> videoInfos, int? totalResults = null)
        {
            videoInfoItems = new ObservableCollection<YouTubeVideoViewModel>(videoInfos);
            if (totalResults.HasValue)
            {
                int resultValue = totalResults.Value;
                pageModel = new PaginationModel(resultValue);
            }
        }

        public void SearchFilter(Dictionary<string, string> parm)
        {
            this.parm = parm;
            presenter.StartSearch(_searchKeyword, parm);
        }

        public void SearchPage(string page)
        {
            pageModel.CurrentPage = page;
            presenter.GetPageList(_searchKeyword, parm, page);
        }
    }
}
