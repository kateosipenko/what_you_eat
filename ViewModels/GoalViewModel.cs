using GalaSoft.MvvmLight.Command;
using Models;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModels.Helpers;

namespace ViewModels
{
    public class GoalViewModel : ViewModel
    {
        private Goal goal = new Goal();
        private int forFood = 0;
        private int forExercises = 0;
        private int maxForFood = 0;
        private int minForExerises = 0;

        public GoalViewModel()
        {
            SetCourseCommand = new RelayCommand<string>(SetCourseExecute);
            SetPeriodCommand = new RelayCommand<string>(SetPeriodExecute);
            GoBackCommand = new RelayCommand(GoBackExecute);
            SetProcentCommand = new RelayCommand(SetProcentExecute);
        }

        #region Properties

        public Goal Goal
        {
            get { return goal; }
            set
            {
                goal = value;
                RaisePropertyChanged("Goal");
            }
        }

        public int MaxForFood
        {
            get { return maxForFood; }
            set
            {
                maxForFood = value;
                RaisePropertyChanged("MaxForFood");
            }
        }

        public int MinForExercises
        {
            get { return minForExerises; }
            set
            {
                minForExerises = value;
                RaisePropertyChanged("MinForExercises");
            }
        }

        public int ForFood
        {
            get { return forFood; }
            set
            {
                forFood = value;
                forExercises = 100 - forFood;
                RaisePropertyChanged("ForFood");
                RaisePropertyChanged("ForExrcises");
            }
        }

        public int ForExrcises
        {
            get { return forExercises; }
            set
            {
                forExercises = value;
                forFood = 100 - forExercises;
                RaisePropertyChanged("ForFood");
                RaisePropertyChanged("ForExrcises");
            }
        }

        #endregion Properties

        #region SetCourseCommand

        public RelayCommand<String> SetCourseCommand { get; private set; }

        private void SetCourseExecute(string course)
        {
            if (course == "next")
            {
                if (goal.Course == Course.LoseWeight || goal.Course == Course.PutOnWeight)
                    NavigationProvider.Navigate(Constants.Pages.DesiredWeight);
                else
                {
                    Diet.SaveGoal(goal);
                    Diet.UpdateDietPlan();
                    string navigationUrl = Constants.Pages.FoodPlan;
                    if (goal.Course == Course.UserPlan)
                        navigationUrl = navigationUrl.AddPageParameter(Constants.NavigationParameters.CanEdit, true);

                    NavigationProvider.Navigate(navigationUrl);
                }
            }
            else if (!string.IsNullOrEmpty(course))
            {
                Goal.Course = (Course)Enum.Parse(typeof(Course), course, true);
            }
        }

        #endregion SetCourseCommand

        #region SetPeriodCommand

        public RelayCommand<string> SetPeriodCommand { get; private set; }

        private void SetPeriodExecute(string parameter)
        {
            switch (parameter)
            {
                case "0":
                    // to lose/ to put on 500 gramm per week
                    goal.DesiredWeeksCount = (int)(Math.Abs(goal.DesiredWeight - Diet.User.BodyState.Weight) / 0.5) + 1;
                    goal.DesiredDate = null;
                    break;
                case "1":
                    goal.DesiredWeeksCount = 0;
                    break;
                case "2":
                    goal.DesiredDate = null;
                    break;
                case "-1":
                    Diet.SaveGoal(goal);
                    if (goal.Course == Course.LoseWeight)
                    {
                        int minCaloriesDif = Diet.Plan.FoodPerDay.NormalPerDay - Diet.Plan.FoodPerDay.CriticalMinimum;
                        float maxForFoodInGramms = (minCaloriesDif / Constants.CaloriesInGrammLose) * 7;
                        MaxForFood = (int) ((maxForFoodInGramms / Diet.Plan.ThrowOffPerWeek) * 100);
                        if (MaxForFood > 100)
                            MaxForFood = 100;
                        MinForExercises = 100 - MaxForFood;
                        ForFood = MaxForFood / 2;
                        NavigationProvider.Navigate(Constants.Pages.LoseWeightPlan);
                    }
                    else
                        NavigationProvider.Navigate(Constants.Pages.FoodPlan);
                    break;
            }
        }

        #endregion SetPeriodCommand

        #region SetProcentCommand

        public RelayCommand SetProcentCommand { get; private set; }

        private void SetProcentExecute()
        {
            Diet.Plan.ProcentForFood = ForFood;
            Diet.Plan.ProcentForTrainings = ForExrcises;
            Diet.UpdateDietPlan();
            NavigationProvider.Navigate(Constants.Pages.FoodPlan);
        }
         
        #endregion SetProcentCommand

        #region GoBackCommand

        public RelayCommand GoBackCommand { get; private set; }

        private void GoBackExecute()
        {
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion GoBackCommand

        #region Cleanup

        public override void Cleanup()
        {
            base.Cleanup();
            this.Goal = null;
        }

        #endregion Cleanup
    }
}
