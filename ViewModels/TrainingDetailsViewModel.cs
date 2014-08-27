using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class TrainingDetailsViewModel : ViewModel
    {
        public TrainingDetailsViewModel()
        {
            AddTrainingCommand = new RelayCommand(AddTrainingExecute);
        }

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

        #region AddTrainingCommand

        public RelayCommand AddTrainingCommand { get; private set; }

        private void AddTrainingExecute()
        {
            Locator.TrainingsPlanStatic.AddTraining(training);
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion AddTrainingCommand

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            Training = new Training();
        }

        protected override void CleanupExecute()
        {
            Training = new Training();
            base.CleanupExecute();
        }
    }
}
