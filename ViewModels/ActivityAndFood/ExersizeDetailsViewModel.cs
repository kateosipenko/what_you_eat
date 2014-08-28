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
        private bool fromPlan = false;

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
            CurrentExersize = new Exersize();
            var parameters = NavigationProvider.GetNavigationParameters();
            fromPlan = parameters.ContainsKey(Constants.NavigationParameters.FromPlan);
            if (parameters.ContainsKey(Constants.NavigationParameters.ActivityId))
            {
                int id;
                int.TryParse(parameters[Constants.NavigationParameters.ActivityId], out id);
                using (var repo = new PhysicalActivityRepository())
                {
                    CurrentExersize.ActivityId = id;
                    CurrentExersize.Activity = repo.GetById(id);
                }
            }
        }

        #endregion Initialization

        #region SpentEneregy

        public RelayCommand SpentEnergyCommend { get; private set; }

        private void SpentEnergyExecute()
        {
            if (fromPlan)
            {
                Locator.TrainingDetailsStatic.AddExersize(CurrentExersize);
            }
            else
            {
                Locator.EnergyTodayStatic.AddEnergy(CurrentExersize);
            }
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion SpentEnergy

        #region Cleanup

        public override void Cleanup()
        {
            fromPlan = false;
            this.CurrentExersize = null;
            base.Cleanup();            
        }

        #endregion Cleanup
    }
}
