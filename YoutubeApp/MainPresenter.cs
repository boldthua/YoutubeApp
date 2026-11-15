using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using YoutubeAPI;
using YoutubeAPI.Channels;
using YoutubeAPI.Models;
using YoutubeAPI.PlayLists;
using YoutubeAPI.Videos;
using YoutubeApp.Models;
using static YoutubeApp.Contracts.MainContract;

namespace YoutubeApp
{
    public class MainPresenter : IMainPresenter
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

        public async Task GetVideoInfo(string itemID)
        {

            var results = await context.video.GetVideoDescription(itemID);

            string channelIds = string.Join(",",
                results.items
                    .Where(x => !string.IsNullOrEmpty(x.snippet?.channelId))
                    .Select(x => x.snippet.channelId)
                    .Distinct());

            ChannelsModel channels =
                await context.channels.GetChannelInfoByChannelIDAsync(channelIds);

            var channelDict = channels?.items?
                .ToDictionary(c => c.id, c => c)
                ?? new Dictionary<string, ChannelsModel.Item>();


            List<YouTubeVideoMapSource> mapSources = new List<YouTubeVideoMapSource>();

            foreach (var video in results.items)
            {
                ChannelsModel.Item channel = null;

                if (!string.IsNullOrEmpty(video.snippet?.channelId))
                {
                    channelDict.TryGetValue(video.snippet.channelId, out channel);
                }

                YouTubeVideoMapSource source = new YouTubeVideoMapSource
                {
                    Video = video,
                    ChannelThumbnailUrl = channel.snippet.thumbnails.medium.url
                };

                mapSources.Add(source);
            }

            List<YouTubeVideoViewModel> videoViewModels = new List<YouTubeVideoViewModel>();

            foreach (var videoChannelSet in mapSources)
            {
                AutoMapper.Mapper mapper = new Mapper();

                YouTubeVideoViewModel vm =
                    mapper.Map<YouTubeVideoViewModel, YouTubeVideoMapSource>(
                        videoChannelSet,
                        exp =>
                        {
                            exp.ForMember(dest => dest.VideoId, src => src.Video.id);
                            exp.ForMember(dest => dest.Title, src => src.Video.snippet.title);
                            exp.ForMember(dest => dest.Duration, src => src.Video.contentDetails.duration);
                            exp.ForMember(dest => dest.ViewCount, src => src.Video.statistics.viewCount);
                            exp.ForMember(dest => dest.PublishedAt, src => src.Video.snippet.publishedAt);
                            exp.ForMember(dest => dest.VideoThumbnailUrl, src => src.Video.snippet.thumbnails.high.url);
                            exp.ForMember(dest => dest.ChannelTitle, src => src.Video.snippet.channelTitle);
                            exp.ForMember(dest => dest.ChannelID, src => src.Video.snippet.channelId);
                            exp.ForMember(dest => dest.ChannelThumbnailUrl, src => src.ChannelThumbnailUrl);
                        });

                videoViewModels.Add(vm);
            }
            view.videoInfoResponse(videoViewModels);
        }

        public async Task StartSearch(string searchText, Dictionary<string, string> parm)
        {
            var result = await context.search.GetPlayListVideoIdAsync(searchText, parm);
            if (result?.items == null || result.items.Length == 0) return;
            string kind = result.items.FirstOrDefault(x => x.id != null)?.id.kind;
            IEnumerable<string> ids;
            switch (kind)
            {
                case "youtube#video":
                    ids = result.items.Select(x => x.id?.videoId);
                    break;
                case "youtube#channel":
                    ids = result.items.Select(x => x.id?.channelId);
                    break;
                case "youtube#playlist":
                    ids = result.items.Select(x => x.id?.playlistId);
                    break;
                default:
                    return;
            }

            string joinedIds = string.Join(",", ids.Where(id => !string.IsNullOrEmpty(id)));
            if (string.IsNullOrEmpty(joinedIds)) return;

            switch (kind)
            {
                case "youtube#video":
                    await GetVideoInfo(joinedIds);
                    break;
                case "youtube#channel":
                    await GetChannelInfo(joinedIds);
                    break;
                case "youtube#playlist":
                    await GetPlayListInfo(joinedIds);
                    break;
            }
        }


        public async Task GetChannelInfo(string joinedChannelIds)
        {
            var channelResult = await context.channels.GetChannelInfoByChannelIDAsync(joinedChannelIds);
            if (channelResult?.items == null || channelResult.items.Length == 0) return;

            view.ChannelInfoResponse(channelResult.items.ToList());
        }

        public async Task GetPlayListInfo(string playListIds)
        {
            // 1. 呼叫 API 直接拿回 PlayListItemsList 物件 (part="snippet,contentDetails")
            PlayListItemsList playlistResult = await context.playList.GetPlayListsAsync(playListIds);

            if (playlistResult?.items == null || playlistResult.items.Length == 0) return;

            view.PlayListInfoResponse(playlistResult);
        }
    }
}