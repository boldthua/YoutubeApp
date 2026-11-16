using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI;
using YoutubeAPI.Models;
using YoutubeAPI.PlayLists;
using YoutubeApp.Models;

namespace YoutubeApp.Contracts
{
    public class VideoSearchContract
    {
        public interface IVedioSearchView
        {
            void SubscriptionItemsResponse(SubscriptionList.Item[] items, string nextPageToken);
            void videoInfoResponse(List<YouTubeVideoViewModel> videoInfos);
            void ChannelInfoResponse(List<ChannelsModel.Item> channelInfos);
            void PlayListInfoResponse(PlayListItemsList playLists);
        }
        public interface IVideoSearchPresenter
        {
            Task GetSubscriptionLists();
            Task GetSubscriptionLists(string pageToken);
            Task StartSearch(string searchText);
            Task StartSearch(string searchText, Dictionary<string, string> parm);
            Task GetVideoInfo(string videoId);
            Task GetChannelInfo(string channelIds);
            Task GetPlayListInfo(string playListIds);
        }
    }
}
