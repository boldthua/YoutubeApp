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
        Frame Frame;
        public void Navigate(string pageName)
        {
            Page page = null;
            switch (pageName)
            {
                case "ShowResultPage":
                    page = new ShowResultPage(); break;
                case "PlayVideoPage":
                    page = new PlayVideoPage(); break;
            }

            Frame.Navigate(page);
        }

        public void SetFrame(Frame frame)
        {
            Frame = frame;
        }
    }
}
