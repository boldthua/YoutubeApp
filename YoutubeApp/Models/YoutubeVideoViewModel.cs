using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeApp.Models
{

    public class YouTubeVideoViewModel
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string ViewCount { get; set; }
        public DateTime PublishedAt { get; set; }

        public string VideoThumbnailUrl { get; set; }

        public ChannelInfo Channel { get; set; }
    }

    public class ChannelInfo
    {
        public string ChannelID { get; set; }
        public string Title { get; set; }
        public string ChannelThumbnailUrl { get; set; }

    }
}
