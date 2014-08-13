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
        #region Fields

        private Goal goal = new Goal();
        private float weightDif;

        #endregion Fields

        public GoalViewModel()
        {
            SaveAndGoNextCommand = new RelayCommand(SaveAndGoNextExecute);
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

        public float WeightDif
        {
            get { return weightDif; }
            set
            {
                weightDif = value;
                RaisePropertyChanged("WeightDif");
            }
        }

        #endregion Properties

        #region SaveAndGoNextCommand

        public RelayCommand SaveAndGoNextCommand { get; private set; }

        private void SaveAndGoNextExecute()
        {
            CacheManager.Instance.SaveGoal(goal, weightDif);
            NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.DistributeCalories.AddPageParameter(Constants.NavigationParameters.FromGoal, true));
        }

        #endregion SaveAndGoNextCommand
    }
}
