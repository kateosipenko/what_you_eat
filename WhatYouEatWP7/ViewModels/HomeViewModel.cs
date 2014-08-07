using DataAccess.Repositories;
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using Models;
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
            NavigateToSpentCommand = new RelayCommand(NavigateToSpentExecute);
        }

        #region NavigateToEatenCommand

        public RelayCommand NavigateToEatenCommand { get; private set; }

        private void NavigateToEatenExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.EatenPage.AddPageParameter(Constants.NavigationParameters.EnergyType, EnergyType.Food));
        }

        #endregion NavigateToEatenCommand

        #region NavigateToSpentCommand

        public RelayCommand NavigateToSpentCommand { get; private set; }

        private void NavigateToSpentExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.TodayActivity.AddPageParameter(Constants.NavigationParameters.EnergyType, EnergyType.Activity));
        }

        #endregion NavigateToSpentCommand

    }
}
