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
    public class TrainingDetailsViewModel : ViewModel
    {
        public TrainingDetailsViewModel()
        {
            SaveTrainingCommand = new RelayCommand(SaveTrainingExecute);
            AddExersizeCommand = new RelayCommand(AddExersizeExecute);
            DeleteExersizeCommand = new RelayCommand<Exersize>(DeleteExersizeExecute);
        }

        #region Exersizes

        private ObservableCollection<Exersize> exersizes = new ObservableCollection<Exersize>();

        public ObservableCollection<Exersize> Exersizes
        {
            get { return exersizes; }
            set
            {
                exersizes = value;
                RaisePropertyChanged("Exersizes");
            }
        }

        #endregion Exersizes

        #region Training

        private Training training = new Training();

        public Type DayType
        {
            get { return typeof(DayOfWeek); }
        }

        public Training Training
        {
            get { return training; }
            set
            {
                training = value;
                RaisePropertyChanged("Training");
            }
        }

        #endregion Training

        #region SaveTrainingCommand

        public RelayCommand SaveTrainingCommand { get; private set; }

        private void SaveTrainingExecute()
        {
            using (var repo = new TrainingRepository())
            {
                var existing = repo.GetById(training.Id);
                if (existing == null)
                {
                    training = repo.Add(training);
                    Locator.TrainingsPlanStatic.AddTraining(training);
                }
            }

            using (var repo = new ExersizesRepository())
            {
                foreach (var exersize in exersizes)
                {
                    exersize.TrainingId = training.Id;
                    exersize.Id = repo.Add(exersize).Id;
                }
            }
            
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion SaveTrainingCommand

        #region AddExersize

        public RelayCommand AddExersizeCommand { get; private set; }

        private void AddExersizeExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.Search.AddPageParameter(Constants.NavigationParameters.EnergyType, EnergyType.Activity)
                .AddPageParameter(Constants.NavigationParameters.FromPlan, true));
        }

        public void AddExersize(Exersize exersize)
        {
            Exersizes.Add(exersize);
            this.Training.Duration = Exersizes.Sum(item => item.Duration);
            float caloriesPerBody = 0;
            using (var activityRepo = new PhysicalActivityRepository())
            {
                var activity = activityRepo.GetById(exersize.ActivityId);
                caloriesPerBody = (float)(Diet.User.BodyState.Weight * activity.Calories) / 60;
                exersize.Activity = activity;
            }

            exersize.CaloriesSpent = (int)(exersize.Duration * caloriesPerBody);
            this.Training.CaloriesMustBurned = Exersizes.Sum(item => item.CaloriesSpent);
        }

        #endregion AddExersize

        #region DeleteExersize

        public RelayCommand<Exersize> DeleteExersizeCommand { get; private set; }

        private void DeleteExersizeExecute(Exersize exersize)
        {
            using (var repo = new ExersizesRepository())
            {
                var existing = repo.GetById(exersize.Id);
                if (existing != null)
                {
                    repo.Delete(existing);
                }
            }

            this.Exersizes.Remove(exersize);
            this.Training.Duration = this.Exersizes.Sum(item => item.Duration);
            this.Training.CaloriesMustBurned = this.Exersizes.Sum(item => item.CaloriesSpent);
        }

        #endregion DeleteExersize

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            var parameters = NavigationProvider.GetNavigationParameters();
            if (parameters.ContainsKey(Constants.NavigationParameters.TrainingId))
            {
                int id;
                int.TryParse(parameters[Constants.NavigationParameters.TrainingId], out id);

                using (var repo = new TrainingRepository())
                {
                    Training = repo.GetById(id);
                }

                using (var repo = new ExersizesRepository())
                {
                    Exersizes = new ObservableCollection<Exersize>(repo.GetByTraining(id));
                }

                using (var repo = new PhysicalActivityRepository())
                {
                    foreach (var item in exersizes)
                    {
                        item.Activity = repo.GetById(item.ActivityId);
                    }
                }
            }

            if (Training == null)
            {
                Training = new Training();
                Exersizes = new ObservableCollection<Exersize>();
            }
        }

        protected override void CleanupExecute()
        {
            Training = new Training();
            Exersizes = new ObservableCollection<Exersize>();
            base.CleanupExecute();
        }
    }
}
