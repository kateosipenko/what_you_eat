using DataAccess.Repositories;
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class HomeViewModel : ViewModel
    {
        public HomeViewModel()
        {
            InitializeViewModelCommand = new RelayCommand(InitializeViewModelExecute);
            NavigateToEatenCommand = new RelayCommand(NavigateToEatenExecute);
        }

        #region NavigateToEatenCommand

        public RelayCommand NavigateToEatenCommand { get; private set; }

        private void NavigateToEatenExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.EatenPage);
        }

        #endregion NavigateToEatenCommand

    }
}
