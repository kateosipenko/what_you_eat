using DataAccess.Repositories;
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class ActivityDetailsViewModel : ViewModel
    {
        #region CurrentActivity

        private PhysicalActivity currentActivity;

        public PhysicalActivity CurrentActivity
        {
            get { return currentActivity; }
            set
            {
                currentActivity = value;
                RaisePropertyChanged("CurrentActivity");
            }
        }

        #endregion CurrentActivity

        public ActivityDetailsViewModel()
        {
            this.InitializeViewModelCommand = new RelayCommand(InitializeViewModelExecute);
            SpentEnergyCommend = new RelayCommand(SpentEnergyExecute);
        }

        #region Initialization

        protected override void InitializeViewModelExecute()
        {
            base.InitializeViewModelExecute();
            BusyCount++;
            var parameters = NavigationProvider.GetNavigationParameters();
            if (parameters.ContainsKey(Constants.NavigationParameters.ActivityId))
            {
                RunInBackground(() =>
                {
                    int id;
                    int.TryParse(parameters[Constants.NavigationParameters.ActivityId], out id);
                    using (var repo = new PhysicalActivityRepository())
                    {
                        currentActivity = repo.GetById(id);
                        InvokeInUIThread(() =>
                        {
                            CurrentActivity = currentActivity;
                            BusyCount--;
                        });
                    }
                });
            }
        }

        #endregion Initialization

        #region SpentEneregy

        public RelayCommand SpentEnergyCommend { get; private set; }

        private void SpentEnergyExecute()
        {
            Locator.EnergyTodayStatic.AddEnergy(CurrentActivity);
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion SpentEnergy
    }
}
