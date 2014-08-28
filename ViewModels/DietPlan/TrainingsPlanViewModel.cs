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
    public class TrainingsPlanViewModel : ViewModel
    {
        public TrainingsPlanViewModel()
        {
            RemoveTrainingCommand = new RelayCommand<Training>(RemoveTrainingExecute);
            AddTrainingCommand = new RelayCommand(AddTrainingExecute);
            GoBackCommand = new RelayCommand(GoBackExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            EditTrainingCommand = new RelayCommand<Training>(EditTrainingExecute);
        }

        #region Trainings

        private ObservableCollection<Training> trainings = new ObservableCollection<Training>();
        public ObservableCollection<Training> Trainings
        {
            get { return trainings; }
            set
            {
                trainings = value;
                RaisePropertyChanged("Trainings");
            }
        }

        #endregion Trainings

        #region MustSpent

        private int mustSpentPerWeek = 0;
        public int MustSpentPerWeek
        {
            get { return mustSpentPerWeek; }
            set
            {
                mustSpentPerWeek = value;
                RaisePropertyChanged("MustSpentPerWeek");
            }
        }

        #endregion MustSpent

        #region TotalCalories

        private int totalCalories = 0;
        public int TotalCalories
        {
            get { return totalCalories; }
            set
            {
                totalCalories = value;
                RaisePropertyChanged("TotalCalories");
            }
        }

        #endregion TotalCalories

        #region RemoveTrainingCommand

        public RelayCommand<Training> RemoveTrainingCommand { get; private set; }

        private void RemoveTrainingExecute(Training training)
        {
            this.Trainings.Remove(training);
            TotalCalories = Trainings.Sum(item => item.CaloriesMustBurned);
            Diet.Plan.Trainigs.Remove(training);
            Diet.SaveDietPlan();
        }

        #endregion RemoveTrainingCommand

        #region AddTrainingCommand

        public RelayCommand AddTrainingCommand { get; private set; }

        private void AddTrainingExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.TrainingPage);
        }

        public void AddTraining(Training training)
        {
            InvokeInUIThread(() =>
            {
                this.Trainings.Add(training);
                TotalCalories = Trainings.Sum(item => item.CaloriesMustBurned);
            });

            Diet.Plan.Trainigs.Add(training);
            Diet.SaveDietPlan();
        }

        #endregion AddTrainingCommand

        #region EditTrainingCommand

        public RelayCommand<Training> EditTrainingCommand { get; private set; }

        private void EditTrainingExecute(Training training)
        {
            NavigationProvider.Navigate(Constants.Pages.TrainingPage.AddPageParameter(Constants.NavigationParameters.TrainingId, training.Id));
        }

        #endregion EditTrainingCommand

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
            Diet.SaveDietPlan();
            NavigationProvider.Navigate(Constants.Pages.WaterPlan);
        }

        #endregion SaveCommand

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            MustSpentPerWeek = Diet.Plan.MustSpentPerWeek;
            Trainings = new ObservableCollection<Training>(Diet.Plan.Trainigs);
            TotalCalories = Trainings.Sum(item => item.CaloriesMustBurned);
        }

        protected override void CleanupExecute()
        {
            MustSpentPerWeek = 0;
            Trainings.Clear();
            TotalCalories = 0;
            base.CleanupExecute();
        }
    }
}
