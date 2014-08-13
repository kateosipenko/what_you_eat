using GalaSoft.MvvmLight.Command;
using Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ViewModels.Helpers;

namespace ViewModels
{
    public class DistributeCaloriesViewModel : ViewModel
    {
        public DistributeCaloriesViewModel()
        {
            InitializeViewModelCommand = new RelayCommand(InitializeViewModelExecute);
            NavigateToHomeCommand = new RelayCommand(NavigateToHomeExecute);
            NavigateToPlanCommand = new RelayCommand(NavigateToPlanExecute);
        }

        #region DietPlan

        private DietPlan dietPlan;

        public DietPlan DietPlan
        {
            get { return dietPlan; }
            set
            {
                dietPlan = value;
                RaisePropertyChanged("DietPlan");
            }
        }

        #endregion DietPlan

        #region Maximum

        private int maximum = 0;

        public int Maximum
        {
            get { return maximum; }
            set
            {
                maximum = value;
                RaisePropertyChanged("Maximum");
            }
        }

        #endregion Maximum

        #region ForFood

        private double forFood;

        public double ForFood
        {
            get { return forFood; }
            set
            {
                forFood = value;
                forExersizes = maximum - forFood;
                dietPlan.CaloriesPerDay = dietPlan.NormalCaloriesPerDay - (int)forFood;
                dietPlan.ExersizesPerDay = (int) forExersizes;
                RaisePropertyChanged("ForFood");
                RaisePropertyChanged("ForExersizes");
            }
        }

        #endregion ForFood

        #region ForExersizes

        private double forExersizes;

        public double ForExersizes
        {
            get { return forExersizes; }
            set
            {
                forExersizes = value;
                forFood = maximum - forExersizes;
                dietPlan.CaloriesPerDay = dietPlan.NormalCaloriesPerDay - (int)forFood;
                dietPlan.ExersizesPerDay = (int) forExersizes;
                RaisePropertyChanged("ForFood");
                RaisePropertyChanged("ForExersizes");
            }
        }

        #endregion ForExersizes

        #region NavigateToHomeCommand

        public RelayCommand NavigateToHomeCommand { get; private set; }

        private void NavigateToHomeExecute()
        {
            CacheManager.Instance.UpdateDietPlan(dietPlan);
            NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.Home);
        }

        #endregion NavigateToHomeCommand

        #region UpdatePlanVisibility

        private Visibility updatePlanVisibility = Visibility.Collapsed;

        public Visibility UpdatePlanVisibility
        {
            get { return updatePlanVisibility; }
            set
            {
                updatePlanVisibility = value;
                RaisePropertyChanged("UpdatePlanVisibility");
            }
        }

        #endregion UpdatePlanVisibility

        #region NavigateToPlanCommand

        public RelayCommand NavigateToPlanCommand { get; private set; }

        private void NavigateToPlanExecute()
        {
            NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.FristStartGoal);
        }

        #endregion NavigateToPlanCommand

        protected override void InitializeViewModelExecute()
        {
            base.InitializeViewModelExecute();
            DietPlan = CacheManager.Instance.Plan;
            switch (CacheManager.Instance.Goal.Course)
            {
                case Course.KeepWeight:
                case Course.PutOnWeight:
                    Maximum = 0;
                    break;
                case Course.LoseWeight:
                    Maximum = DietPlan.UselessCalories;
                    break;
            }

            var parameters = NavigationProvider.GetNavigationParameters();
            if (parameters.ContainsKey(Constants.NavigationParameters.FromGoal))
            {
                UpdatePlanVisibility = Visibility.Collapsed;
                ForExersizes = Maximum / 2;
            }
            else
            {
                UpdatePlanVisibility = Visibility.Visible;
                ForFood = CacheManager.Instance.Goal.ForFood;
            }
        }
    }
}
