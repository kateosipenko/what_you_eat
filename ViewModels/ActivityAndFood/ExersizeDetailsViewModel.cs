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
    public class ExersizeDetailsViewModel : ViewModel
    {
        #region CurrentExersize

        private Exersize currentExersize;

        public Exersize CurrentExersize
        {
            get { return currentExersize; }
            set
            {
                currentExersize = value;
                RaisePropertyChanged("CurrentExersize");
            }
        }

        #endregion CurrentExersize

        public ExersizeDetailsViewModel()
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
                int id;
                int.TryParse(parameters[Constants.NavigationParameters.ActivityId], out id);
                CurrentExersize.ActivityId = id;
            }
        }

        #endregion Initialization

        #region SpentEneregy

        public RelayCommand SpentEnergyCommend { get; private set; }

        private void SpentEnergyExecute()
        {
            Locator.EnergyTodayStatic.AddEnergy(CurrentExersize);
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion SpentEnergy

        #region Cleanup

        public override void Cleanup()
        {
            base.Cleanup();
            this.CurrentExersize = null;
        }

        #endregion Cleanup
    }
}
