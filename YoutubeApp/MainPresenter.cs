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

        public async Task GetSearch(string searchText)
        {
            var result = await context.search.GetPlayListVideoIdAsync(searchText);
            view.SearchResponse(result);
        }
    }
}
