using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI;
using YoutubeAPI.Models;
using static YoutubeApp.Contracts.MainContract;

namespace YoutubeApp
{
    internal class MainPresenter : IMainPresenter
    {
        IMainView view;
        YoutubeContext context = new YoutubeContext();
        public MainPresenter(IMainView view)
        {
            this.view = view;
        }

        public async Task GetMyAccountInfo()
        {
            var result = await context.channels.GetMyChannelInfoAsync();
            view.MyAccountResponse(result.items[0]);
        }

        public async Task GetSubscriptionLists()
        {
            var result = await context.subscription.GetMySubscriptionListAsync();
            view.SubscriptionItemsResponse(result.items, result.nextPageToken);
        }

        public async Task GetSubscriptionLists(string pageToken)
        {
            var result = await context.subscription.GetMySubscriptionListAsync(pageToken);
            view.SubscriptionItemsResponse(result.items, result.nextPageToken);
        }

        public async Task StartSearch(string searchText)
        {
            var result = await context.search.GetPlayListVideoIdAsync(searchText);
            if (result?.items == null || result.items.Length == 0) return;

            string joinedIds = string.Join(",", result.items
                                     .Where(x => !string.IsNullOrEmpty(x.id?.videoId))
                                     .Select(x => x.id.videoId));

            await GetVideoInfo(joinedIds);
        }

        public async Task GetVideoInfo(string videoId)
        {
            var result = await context.video.GetVideoDescription(videoId);
            view.videoInfoResponse(result);
        }
    }
}
