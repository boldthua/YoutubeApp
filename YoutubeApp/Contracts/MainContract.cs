using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Models;
using YoutubeApp.Models;

namespace YoutubeApp.Contracts
{
    public class MainContract
    {
        public interface IMainView
        {
            void SubscriptionItemsResponse(SubscriptionList.Item[] items, string nextPageToken);
            void MyAccountResponse(ChannelsModel.Item myAccount);
            void videoInfoResponse(List<YouTubeVideoViewModel> videoInfos);
        }
        public interface IMainPresenter
        {
            Task GetSubscriptionLists();
            Task GetSubscriptionLists(string pageToken);
            Task GetMyAccountInfo();
            Task StartSearch(string searchText);
            Task GetVideoInfo(string videoId);
        }
    }
}
