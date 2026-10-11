using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeApp.Models
{
    public class PaginationModel
    {
        public string CurrentPage { get; set; } = "1";
        public int TotalPage { get; set; }

        public PaginationModel(int totalResults) 
        {
            TotalPage = (totalResults + 49) / 50;
        }
    }
}
