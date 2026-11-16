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
using static YoutubeApp.Contracts.VideoSearchContract;

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

    }
}