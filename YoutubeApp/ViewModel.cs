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
using YoutubeApp.Utilities;
using static YoutubeApp.Contracts.MainContract;

namespace YoutubeApp
{

    [AddINotifyPropertyChangedInterface]
    internal class ViewModel : IMainView
    {
        public ObservableCollection<SubscriptionList.Item> myFavorites { get; set; } = new ObservableCollection<SubscriptionList.Item>();
        IMainPresenter presenter { get; set; }
        public string nextPageToken { get; set; }
        public string searchKeyword { get; set; }
        public ObservableCollection<GetVideoModel.Item> videoInfoItems { get; set; } = new ObservableCollection<GetVideoModel.Item>();
        public bool IsMoreSubscrib { get; set; } = false;
        public ICommand moreSubscribCommand { get; set; }
        public ICommand searchCommand { get; set; }
        public ChannelsModel.Item myAccount { get; set; }

        public ViewModel()
        {
            presenter = new MainPresenter(this);
            presenter.GetSubscriptionLists();
            moreSubscribCommand = new RelayCommand(GetMoreSubcrib);
            searchCommand = new RelayCommand(startSearch);
            presenter.GetMyAccountInfo();
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
        public void startSearch()
        {
            presenter.StartSearch(searchKeyword);
        }

        public void GetMoreSubcrib()
        {
            presenter.GetSubscriptionLists(nextPageToken);
        }

        public void MyAccountResponse(ChannelsModel.Item myAccount)
        {
            this.myAccount = myAccount;
        }

        public void videoInfoResponse(GetVideoModel videoInfo)
        {
            videoInfoItems = new ObservableCollection<GetVideoModel.Item>(videoInfo.items);
        }
    }
}
