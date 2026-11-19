using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using YoutubeApp.Pages;

namespace YoutubeApp.Utilities
{
    public class NavigationService : INavigationService
    {
        Frame Frame = new Frame();
        public void Navigate(string pageName)
        {
            Page page = null;
            switch (pageName)
            {
                case "ShowResultPage":
                    page = new ShowResultPage(this); break;
                case "PlayVideoPage":
                    page = new PlayVideoPage(this); break;
            }

            Frame.Navigate(page);
        }

        public void SetFrame(Frame frame)
        {
            Frame = frame;
        }
    }
}
