using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class SelectedOption
    {
        public string Text { get; set; }
        public string EngText { get; set; }
        public bool IsChecked { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public SelectedOption(string name, string engText)
        {
            Text = name;
            EngText = engText;
        }
    }
}
