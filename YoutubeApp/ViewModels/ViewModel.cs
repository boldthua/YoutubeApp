using AutoMapper;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeAPI;
using YoutubeApp.Contracts;
using YoutubeAPI.Models;
using YoutubeAPI.PlayLists;
using YoutubeAPI.Videos;
using YoutubeApp.Models;
using YoutubeApp.Utilities;
using static YoutubeApp.Contracts.VideoSearchContract;
using static YoutubeApp.Contracts.MainContract;

namespace YoutubeApp
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel : IMainView
    {
        public IMainPresenter presenter { get; set; }
        public string _searchKeyword;
        public bool IsSearchKeyWordExist { get; set; }
        public ICommand searchCommand { get; set; }

        public ChannelsModel.Item myAccount { get; set; }

        public ViewModel(INavigationService navigation)
        {
            // 一開始就要戴入 ShowResultPage
            navigation.Navigate("ShowResultPage");

            presenter = new MainPresenter(this);
            searchCommand = new RelayCommand(async () =>
            {
                
            });
            presenter.GetMyAccountInfo();

            //
        }

        public string SearchKeyWord
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                IsSearchKeyWordExist = !string.IsNullOrWhiteSpace(value);
            }
        }

        public void MyAccountResponse(ChannelsModel.Item myAccount)
        {
            this.myAccount = myAccount;
        }
    }
}
