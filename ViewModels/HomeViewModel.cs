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
using ViewModels.Helpers;

namespace ViewModels
{
    public class HomeViewModel : ViewModel
    {
        public HomeViewModel()
        {
            InitializeViewModelCommand = new RelayCommand(InitializeViewModelExecute);
            NavigateToEatenCommand = new RelayCommand(NavigateToEatenExecute);
            NavigateToSpentCommand = new RelayCommand(NavigateToSpentExecute);
            NavigateToPlanCommand = new RelayCommand(NavigateToPlanExecute);
        }

        protected override void InitializeViewModelExecute()
        {
            base.InitializeViewModelExecute();
            TotalEaten = CacheManager.Instance.GetEatenToday().Sum(item => item.AmountOfCalories);
            TotalActivity = CacheManager.Instance.GetSpentToday().Sum(item => item.SpentEnergy);
            MustEat = CacheManager.Instance.Plan.CaloriesPerDay;
            MustSpent = CacheManager.Instance.Plan.ExersizesPerDay;
        }

        #region Food

        private int mustEat = 0;
        private int totalEaten = 0;

        public int MustEat
        {
            get { return mustEat; }
            set
            {
                mustEat = value;
                RaisePropertyChanged("MustEat");
            }
        }

        public int TotalEaten
        {
            get { return totalEaten; }
            set
            {
                totalEaten = value;
                RaisePropertyChanged("TotalEaten");
            }
        }

        #endregion Food

        #region Activity

        private int totalActivity;
        private int mustSpent = 0;

        public int MustSpent
        {
            get { return mustSpent; }
            set
            {
                mustSpent = value;
                RaisePropertyChanged("MustSpent");
            }
        }

        public int TotalActivity
        {
            get { return totalActivity; }
            set
            {
                totalActivity = value;
                RaisePropertyChanged("TotalActivity");
            }
        }

        #endregion Activity

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

        #region NavigateToPlanCommand

        public RelayCommand NavigateToPlanCommand { get; private set; }

        private void NavigateToPlanExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.DistributeCalories);
        }

        #endregion NavigateToPlanCommand
    }
}
