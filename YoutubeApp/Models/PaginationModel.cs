using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeApp.Models
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPage { get; set; }
        public ObservableCollection<string> pageSet { get; set; }

        public PaginationModel(int totalResults)
        {
            TotalPage = (totalResults + 49) / 50;
            ChangePageSet(CurrentPage);
        }

        public void ChangePageSet(int pageToGo)
        {
            int firstPageNum = ((pageToGo - 1) / 10) * 10 + 1;
            pageSet.Clear();
            int lastPageNum = Math.Min(firstPageNum + 9, TotalPage);
            for (int i = firstPageNum; i <= lastPageNum; i++)
            {
                pageSet.Add(i.ToString());
            }

        }
    }
}
