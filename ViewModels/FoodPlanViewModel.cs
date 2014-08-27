using GalaSoft.MvvmLight.Command;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class FoodPlanViewModel : ViewModel
    {
        public FoodPlanViewModel()
        {
            this.GoBackCommand = new RelayCommand(GoBackExecute);
            this.SaveCommand = new RelayCommand(SaveExecute);
        }

        #region CanEdit

        private bool canEdit = false;

        public bool CanEdit
        {
            get { return canEdit; }
            set
            {
                canEdit = value;
                RaisePropertyChanged("CanEdit");
            }
        }

        #endregion CanEdit

        #region CaloriesPerDay

        private int caloriesPerDay;

        public int CaloriesPerDay
        {
            get { return caloriesPerDay; }
            set
            {
                caloriesPerDay = value;
                RaisePropertyChanged("CaloriesPerDay");
            }
        }

        #endregion CaloriesPerDay

        #region Proteins

        private int protein;
        public int Protein
        {
            get { return protein; }
            set
            {
                protein = value;
                RaisePropertyChanged("Protein");
            }
        }

        #endregion Proteins

        #region Fats

        private int fats;
        public int Fats
        {
            get { return fats; }
            set
            {
                fats = value;
                RaisePropertyChanged("Fats");
            }
        }

        #endregion Fats

        #region Carbohydrates

        private int carbohydrates;
        public int Carbohydrates
        {
            get { return carbohydrates; }
            set
            {
                carbohydrates = value;
                RaisePropertyChanged("Carbohydrates");
            }
        }

        #endregion Carbohydrates

        #region MealsCount

        private int mealsCount = 0;
        public int MealsCount
        {
            get { return mealsCount; }
            set
            {
                mealsCount = value;
                RaisePropertyChanged("MealsCount");
            }
        }

        #endregion MealsCount

        #region GoBackCommand

        public RelayCommand GoBackCommand { get; private set; }

        private void GoBackExecute()
        {
            if (NavigationProvider.CanGoBack())
            {
                NavigationProvider.GoBack();
            }
        }

        #endregion GoBackCommand

        #region SaveCommand

        public RelayCommand SaveCommand { get; private set; }

        private void SaveExecute()
        {
            var plan = Diet.Plan;
            plan.FoodPerDay.Fats = Fats;
            plan.FoodPerDay.DailyCalories = CaloriesPerDay;
            plan.FoodPerDay.Carbohydrates = Carbohydrates;
            plan.FoodPerDay.Protein = Protein;
            plan.FoodPerDay.MealsCount = MealsCount;
            Diet.SaveDietPlan();
            NavigationProvider.Navigate(Constants.Pages.TrainingsPlan);
        }

        #endregion SaveCommand

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            var parameters = NavigationProvider.GetNavigationParameters();
            CanEdit = parameters.ContainsKey(Constants.NavigationParameters.CanEdit);
            CaloriesPerDay = Diet.Plan.FoodPerDay.DailyCalories;
            Protein = Diet.Plan.FoodPerDay.Protein;
            Fats = Diet.Plan.FoodPerDay.Fats;
            Carbohydrates = Diet.Plan.FoodPerDay.Carbohydrates;
            MealsCount = Diet.Plan.FoodPerDay.MealsCount;
        }

        protected override void CleanupExecute()
        {
            CanEdit = false;
            CaloriesPerDay = 0;
            Protein = 0;
            Fats = 0;
            Carbohydrates = 0;
            base.CleanupExecute();
        }
    }
}
