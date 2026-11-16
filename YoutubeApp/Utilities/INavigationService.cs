using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace YoutubeApp.Utilities
{
    public interface INavigationService
    {
        void Navigate(string pageName);

        void SetFrame(Frame frame);
    }
}
