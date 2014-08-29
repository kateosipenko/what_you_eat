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
            NavigateToEatenCommand = new RelayCommand(NavigateToEatenExecute);
            NavigateToSpentCommand = new RelayCommand(NavigateToSpentExecute);
            NavigateToPlanCommand = new RelayCommand(NavigateToPlanExecute);
            NavigateToSettingsCommand = new RelayCommand(NavigateToSettingsExecute);
            NavigateToProfileCommand = new RelayCommand(NavigateToProfileExecute);
            NavigateToWaterCommand = new RelayCommand(NavigateToWater);
            NavigateToFoodPlanCommand = new RelayCommand(NavigateToFoodPlanExecute);
            NavigateToTrainingsPlanCommand = new RelayCommand(NavigateToTrainingsPlanExecute);
            NavigateToWaterPlanCommand = new RelayCommand(NavigateToWaterPlanExecute);
        }

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            TotalEaten = Diet.GetEatenToday().Sum(item => item.AmountOfCalories);
            TotalActivity = Diet.GetSpentToday().Sum(item => item.CaloriesSpent);
            MustEat = Diet.Plan.FoodPerDay.DailyCalories;
            MustSpent = Diet.GetMustSpentToday();
            MustDrink = Diet.Plan.WaterPlan.Amount;
            WaterToday = Diet.WaterToday;
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

        #region Water

        private int mustDrink = 0;
        private int waterToday = 0;

        public int MustDrink
        {
            get { return mustDrink; }
            set
            {
                mustDrink = value;
                RaisePropertyChanged("MustDrink");
            }
        }

        public int WaterToday
        {
            get { return waterToday; }
            set
            {
                waterToday = value;
                RaisePropertyChanged("WaterToday");
            }
        }

        #endregion Water

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
            NavigationProvider.Navigate(Constants.Pages.FristStartGoal);
        }

        #endregion NavigateToPlanCommand

        #region NavigateToSettingsCommand

        public RelayCommand NavigateToSettingsCommand { get; private set; }

        private void NavigateToSettingsExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.Settings);
        }

        #endregion NavigateToSettingsCommand

        #region NavigateToProfileCommand

        public RelayCommand NavigateToProfileCommand { get; private set; }

        private void NavigateToProfileExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.Profile);
        }

        #endregion NavigateToProfileCommand

        #region NavigateToWaterCommand

        public RelayCommand NavigateToWaterCommand { get; private set; }

        private void NavigateToWater()
        {
            NavigationProvider.Navigate(Constants.Pages.Water);
        }

        #endregion NavigateToWaterCommand

        #region NavigateToFoodPlanCommand

        public RelayCommand NavigateToFoodPlanCommand { get; private set; }

        private void NavigateToFoodPlanExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.FoodPlan.AddPageParameter(Constants.NavigationParameters.FromHome, true));
        }

        #endregion NavigateToFoodPlanCommand

        #region NavigateToTrainingsPlanCommand

        public RelayCommand NavigateToTrainingsPlanCommand { get; private set; }

        private void NavigateToTrainingsPlanExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.TrainingsPlan.AddPageParameter(Constants.NavigationParameters.FromHome, true));
        }

        #endregion NavigateToTrainingsPlanCommand

        #region NavigateToWaterPlanCommand

        public RelayCommand NavigateToWaterPlanCommand { get; private set; }

        private void NavigateToWaterPlanExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.WaterPlan.AddPageParameter(Constants.NavigationParameters.FromHome, true));
        }

        #endregion NavigateToWaterPlanCommand
    }
}