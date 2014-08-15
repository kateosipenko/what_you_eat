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
            SpentEnergyCommend = new RelayCommand(SpentEnergyExecute);
        }

        #region Initialization

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
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

        #region Cleanup

        public override void Cleanup()
        {
            base.Cleanup();
            this.CurrentActivity = null;
        }

        #endregion Cleanup
    }
}
