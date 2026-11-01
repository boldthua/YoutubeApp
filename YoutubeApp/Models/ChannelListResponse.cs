using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeApp.Models
{
    public class ChannelListResponse
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string prevPageToken { get; set; }
        public Pageinfo pageInfo { get; set; }
        public List<YoutubeChannel> items { get; set; }
    }


    public class Pageinfo
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }


}
