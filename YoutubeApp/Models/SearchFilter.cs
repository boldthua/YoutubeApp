using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YoutubeAPI.PlayLists;

namespace YoutubeApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class SearchFilter
    {
        public ObservableCollection<SelectedOption> Type { get; set; } = new ObservableCollection<SelectedOption>()
        {
            new SelectedOption("影片", "video"), new SelectedOption("頻道", "channel"), new SelectedOption("播放清單", "playlist")
        };
        public ObservableCollection<SelectedOption> Duration { get; set; } = new ObservableCollection<SelectedOption>()
        {
             new SelectedOption("超過 20 分鐘", "long"), new SelectedOption("3 到 20 分鐘", "medium"), new SelectedOption("3 分鐘內", "short")
        };
        public ObservableCollection<SelectedOption> UpLoadPeriod { get; set; } = new ObservableCollection<SelectedOption>()
        {
             new SelectedOption("今天", "今天"),  new SelectedOption("本週", "本週"),  new SelectedOption("本月", "本月"),  new SelectedOption("今年", "今年")
        };
        public ObservableCollection<SelectedOption> Order { get; set; } = new ObservableCollection<SelectedOption>()
        {
             new SelectedOption("關聯性", "relevance"),  new SelectedOption("頻道熱門程度", "viewCount")
        };


    }
}
