using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeAPI;
using AutoMapper;
using YoutubeAPI.Models;
using YoutubeApp.Models;
using static YoutubeApp.Contracts.MainContract;
using System.Runtime.CompilerServices;

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

        public async Task GetVideoInfo(string videoId)
        {
            GetVideoModel results = await context.video.GetVideoDescription(videoId);

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
                    Channel = channel
                };

                mapSources.Add(source);
            }

            List<YouTubeVideoViewModel> videoViewModels =
    new List<YouTubeVideoViewModel>();

            foreach (var videoChannelSet in mapSources)
            {
                AutoMapper.Mapper mapper = new Mapper();

                YouTubeVideoViewModel vm =
                    mapper.Map<YouTubeVideoViewModel, YouTubeVideoMapSource>(
                        videoChannelSet,
                        exp =>
                        {
                            exp.ForMember(
                                dest => dest.VideoId,
                                src => src.Video.id
                            );

                            exp.ForMember(
                                dest => dest.Title,
                                src => src.Video.snippet.title
                            );

                            exp.ForMember(
                                dest => dest.Duration,
                                src => src.Video.contentDetails.duration
                            );

                            exp.ForMember(
                                dest => dest.ViewCount,
                                src => src.Video.statistics.viewCount
                            );

                            exp.ForMember(
                                dest => dest.PublishedAt,
                                src => src.Video.snippet.publishedAt
                            );

                            exp.ForMember(
                                dest => dest.VideoThumbnailUrl,
                                src => src.Video.snippet.thumbnails.high.url
                            );
                        });

                videoViewModels.Add(vm);
            }

            view.videoInfoResponse(videoViewModels);

        }
    }
}