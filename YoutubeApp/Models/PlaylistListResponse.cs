using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeApp.Models
{
    public class YoutubePlaylistListResponse
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string prevPageToken { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<YoutubePlaylist> items { get; set; }

        public class PageInfo
        {
            public int totalResults { get; set; }
            public int resultsPerPage { get; set; }
        }
    }
}
