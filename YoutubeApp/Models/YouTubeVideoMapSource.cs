using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Models;

namespace YoutubeApp.Models
{
    public class YouTubeVideoMapSource
    {
        public GetVideoModel.Item Video { get; set; }

        public ChannelsModel.Item Channel { get; set; }
    }
}
