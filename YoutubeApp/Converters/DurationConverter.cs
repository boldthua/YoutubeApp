using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml;

namespace YoutubeApp.Converters
{
    // 類別 1：影片長度轉換器
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string durationStr && !string.IsNullOrEmpty(durationStr))
            {
                try
                {
                    // 利用 .NET 內建的 XmlConvert 自動解析 ISO 8601 時間字串
                    TimeSpan time = XmlConvert.ToTimeSpan(durationStr);

                    // 如果超過 1 小時，顯示 H:mm:ss；否則顯示 m:ss
                    if (time.TotalHours >= 1)
                        return $"{(int)time.TotalHours}:{time.Minutes:D2}:{time.Seconds:D2}";
                    else
                        return $"{time.Minutes}:{time.Seconds:D2}";
                }
                catch
                {
                    return "0:00"; // 解析失敗時的安全防護
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // 類別 2：觀看次數轉換器
    public class ViewCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string countStr && long.TryParse(countStr, out long count))
            {
                // 台灣習慣的「萬」單位換算
                if (count >= 10000)
                {
                    // 1310000 -> 131萬
                    // 1315000 -> 131.5萬 (保留一位小數)
                    return $"{count / 10000.0:0.#}萬次觀看";
                }
                return $"{count}次觀看";
            }
            return "無觀看次數";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // 類別 3：發布時間轉換器 (例如：1年前)
    public class PublishedTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 確保傳進來的值是 DateTime 格式
            if (value is DateTime publishedTime)
            {
                // 取得目前時間
                DateTime now = DateTime.Now;

                // 🌟 注意：如果 YouTube API 回傳的是 UTC 時間，而你的電腦是本地時間
                // 保險起見，可以將現在時間轉換為 UTC 再計算，或者將 API 時間轉換為本地時間。
                // 這裡我們假設你收到的 publishedAt 已經是正確時區的 DateTime，或者直接使用 UTC 計算最安全：
                // DateTime now = DateTime.UtcNow; 
                // 如果你的時間算出來怪怪的（例如未來時間），請把上面這行註解解開並替換掉。

                // 計算兩個時間的差距
                TimeSpan diff = now - publishedTime;

                // 處理可能出現的極小負數（例如兩邊時區剛好有幾毫秒落差）
                if (diff.TotalSeconds < 0)
                {
                    return "剛剛";
                }

                // 依照差距大小，回傳不同的中文字串 (依照時間單位由大到小判斷)
                int diffDays = (int)diff.TotalDays;

                if (diffDays >= 365)
                {
                    int years = diffDays / 365;
                    return $"{years} 年前";
                }
                else if (diffDays >= 30)
                {
                    int months = diffDays / 30;
                    return $"{months} 個月前";
                }
                else if (diffDays >= 7)
                {
                    int weeks = diffDays / 7;
                    return $"{weeks} 週前";
                }
                else if (diffDays >= 1)
                {
                    return $"{diffDays} 天前";
                }
                else if (diff.TotalHours >= 1)
                {
                    return $"{(int)diff.TotalHours} 小時前";
                }
                else if (diff.TotalMinutes >= 1)
                {
                    return $"{(int)diff.TotalMinutes} 分鐘前";
                }
                else
                {
                    return "剛剛";
                }
            }

            // 如果傳進來的不是 DateTime，或者為 null，就回傳空字串
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("時間只能單向轉換，不需要實作 ConvertBack");
        }
    }
}
