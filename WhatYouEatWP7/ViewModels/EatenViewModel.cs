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
    public class EatenViewModel : ViewModel
    {
        public EatenViewModel()
        {
            NavigateToFoodSearchCommand = new RelayCommand(NavigateToFoodSearchExecute);
            InitializeViewModelCommand = new RelayCommand(InitializeViewModelExecute);
            DeleteFromEatenCommand = new RelayCommand<Food>(DeleteFromEatenExecute);
        }

        protected override void InitializeViewModelExecute()
        {
            base.InitializeViewModelExecute();
            BusyCount++;
            RunInBackground(() =>
            {
                var eaten = CacheManager.Instance.GetEatenToday();
                InvokeInUIThread(() =>
                {
                    EatenToday = new ObservableCollection<Food>(eaten);
                    TotalEaten = EatenToday.Sum(item => item.AmountOfCalories);
                    BusyCount--;
                });
            });
        }

        #region TotalEaten

        private int totalEaten = 0;

        public int TotalEaten
        {
            get { return totalEaten; }
            set
            {
                totalEaten = value;
                RaisePropertyChanged("TotalEaten");
            }
        }

        #endregion TotalEaten

        #region NavigateToFoodSearchCommand

        public RelayCommand NavigateToFoodSearchCommand { get; private set; }

        private void NavigateToFoodSearchExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.FoodSearch);
        }

        #endregion NavigateToFoodSearchCommand

        #region EatenFoodToday

        private ObservableCollection<Food> eatenToday = new ObservableCollection<Food>();

        public ObservableCollection<Food> EatenToday
        {
            get { return eatenToday; }
            set
            {
                eatenToday = value;
                RaisePropertyChanged("EatenToday");
            }
        }

        #endregion EatenFoodToday

        #region AddOrDeleteEaten

        public void AddEatenFood(Food food)
        {
            BusyCount++;
            RunInBackground(() =>
            {
                var eaten = CacheManager.Instance.EatFood(food);
                InvokeInUIThread(() =>
                {
                    EatenToday.Add(eaten);
                    TotalEaten = EatenToday.Sum(item => item.AmountOfCalories);
                    BusyCount--;
                });
            });
        }

        public RelayCommand<Food> DeleteFromEatenCommand { get; private set; }

        private void DeleteFromEatenExecute(Food food)
        {
            BusyCount++;
            RunInBackground(() =>
            {
                CacheManager.Instance.DeleteEatenFood(food);
                InvokeInUIThread(() =>
                {
                    EatenToday.Remove(food);
                    TotalEaten = EatenToday.Sum(item => item.AmountOfCalories);
                    BusyCount--;
                });
            });
        }

        #endregion AddOrDeleteEaten
    }
}
