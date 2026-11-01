using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YoutubeApp.Models.YoutubeChannel;

namespace YoutubeApp.Models
{
    public class YoutubePlaylist
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet snippet { get; set; }
        public Status status { get; set; }
        public Contentdetails contentDetails { get; set; }
        public Player player { get; set; }
        public Localizations localizations { get; set; }

        public class Snippet
        {
            public DateTime publishedAt { get; set; }
            public string channelId { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public Thumbnails thumbnails { get; set; }
            public string channelTitle { get; set; }
            public string defaultLanguage { get; set; }
            public Localized localized { get; set; }
        }

        public class Status
        {
            public string privacyStatus { get; set; }
            public string podcastStatus { get; set; }
        }

        public class Contentdetails
        {
            public int itemCount { get; set; }
        }

        public class Player
        {
            public string embedHtml { get; set; }
        }
    }
}
