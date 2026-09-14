using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeApp.Models;
using YoutubeApp.Utilities;

namespace YoutubeApp.Components.SearchFilterControl
{
    [AddINotifyPropertyChangedInterface]
    public class SearchFilterContext
    {
        public ICommand FilterCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public Models.SearchFilter searchFilter { get; set; } = new Models.SearchFilter();

        public SearchFilterContext()
        {
            FilterCommand = new RelayCommand(GatherOptionsAndSearch);
        }

        public async void GatherOptionsAndSearch()
        {
            Dictionary<string, string> parm = new Dictionary<string, string>() {
                {"type", searchFilter.Type.FirstOrDefault(x => x.IsChecked)?.EngText ?? null},
                {"videoDuration", searchFilter.Duration.FirstOrDefault(x => x.IsChecked)?.EngText ?? null },
                {"publishedAfter", DateConverter((searchFilter.UpLoadPeriod.FirstOrDefault(x => x.IsChecked)?.EngText ?? null))},
                {"order", searchFilter.Order.FirstOrDefault(x=> x.IsChecked)?.EngText ?? null}
            }.Where(x => !string.IsNullOrEmpty(x.Value))
             .ToDictionary(x => x.Key, x => x.Value);
            //await presenter.StartSearch((_searchKeyword), parm);

            this.SearchCommand?.Execute(parm);
        }

        public string DateConverter(string periodText)
        {
            if (periodText == "")
                return "";
            DateTime today = DateTime.Today;
            DateTime? localDate = null;
            switch (periodText)
            {
                case "今天":
                    localDate = today;
                    break;
                case "本週":
                    localDate = today.AddDays(-(int)today.DayOfWeek); // 本週第一天 (週日)
                    break;
                case "本月":
                    localDate = new DateTime(today.Year, today.Month, 1);
                    break;
                case "今年":
                    localDate = new DateTime(today.Year, 1, 1);
                    break;
                default:
                    return null;
            }

            if (!localDate.HasValue)
                return null;

            // 轉為 UTC 時間並輸出為 "yyyy-MM-ddTHH:mm:ssZ" 格式 (如 "2026-08-01T00:00:00Z")
            return DateTime.SpecifyKind(localDate.Value, DateTimeKind.Local)
                           .ToUniversalTime()
                           .ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
