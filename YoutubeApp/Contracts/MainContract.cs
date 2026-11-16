using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Models;

namespace YoutubeApp.Contracts
{
    public class MainContract
    {
        public interface IMainView
        {
            void MyAccountResponse(ChannelsModel.Item myAccount);
        }
        public interface IMainPresenter
        {
            Task GetMyAccountInfo();
        }
    }
}
