using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeApp.Models
{
    public class YoutubeChannel
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet snippet { get; set; }
        public Contentdetails contentDetails { get; set; }
        public Statistics statistics { get; set; }
        public Topicdetails topicDetails { get; set; }
        public Status status { get; set; }
        public Brandingsettings brandingSettings { get; set; }
        public Auditdetails auditDetails { get; set; }
        public Contentownerdetails contentOwnerDetails { get; set; }
        public Localizations localizations { get; set; }

        public class Snippet
        {
            public string title { get; set; }
            public string description { get; set; }
            public string customUrl { get; set; }
            public DateTime publishedAt { get; set; }
            public Thumbnails thumbnails { get; set; }
            public string defaultLanguage { get; set; }
            public Localized localized { get; set; }
            public string country { get; set; }
        }

        public class Thumbnails
        {
            public Default _default { get; set; }
        }

        public class Default
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Localized
        {
            public string title { get; set; }
            public string description { get; set; }
        }

        public class Contentdetails
        {
            public Relatedplaylists relatedPlaylists { get; set; }
        }

        public class Relatedplaylists
        {
            public string likes { get; set; }
            public string favorites { get; set; }
            public string uploads { get; set; }
        }

        public class Statistics
        {
            public string viewCount { get; set; }
            public string subscriberCount { get; set; }
            public bool hiddenSubscriberCount { get; set; }
            public string videoCount { get; set; }
        }

        public class Topicdetails
        {
            public string[] topicIds { get; set; }
            public string[] topicCategories { get; set; }
        }

        public class Status
        {
            public string privacyStatus { get; set; }
            public bool isLinked { get; set; }
            public string longUploadsStatus { get; set; }
            public bool madeForKids { get; set; }
            public bool selfDeclaredMadeForKids { get; set; }
        }

        public class Brandingsettings
        {
            public Channel channel { get; set; }
            public Watch watch { get; set; }
        }

        public class Channel
        {
            public string title { get; set; }
            public string description { get; set; }
            public string keywords { get; set; }
            public string trackingAnalyticsAccountId { get; set; }
            public string unsubscribedTrailer { get; set; }
            public string defaultLanguage { get; set; }
            public string country { get; set; }
        }

        public class Watch
        {
            public string textColor { get; set; }
            public string backgroundColor { get; set; }
            public string featuredPlaylistId { get; set; }
        }

        public class Auditdetails
        {
            public bool overallGoodStanding { get; set; }
            public bool communityGuidelinesGoodStanding { get; set; }
            public bool copyrightStrikesGoodStanding { get; set; }
            public bool contentIdClaimsGoodStanding { get; set; }
        }

        public class Contentownerdetails
        {
            public string contentOwner { get; set; }
            public DateTime timeLinked { get; set; }
        }

        public class Localizations
        {
            public En en { get; set; }
        }

        public class En
        {
            public string title { get; set; }
            public string description { get; set; }
        }

    }
}
