using GalaSoft.MvvmLight.Command;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class WaterPlanViewModel : ViewModel
    {
        public WaterPlanViewModel()
        {
            GoBackCommand = new RelayCommand(GoBackExecute);
            SaveCommand = new RelayCommand(SaveExecute);
        }

        #region Amount

        private int amount = 0;
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                RaisePropertyChanged("Amount");
            }
        }

        #endregion Amount

        #region IntakeCount

        private int intakeCount = 0;
        public int IntakeCount
        {
            get { return intakeCount; }
            set
            {
                intakeCount = value;
                RaisePropertyChanged("IntakeCount");
            }
        }

        #endregion IntakeCount

        #region GoBackCommand

        public RelayCommand GoBackCommand { get; private set; }

        private void GoBackExecute()
        {
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion GoBakCommand

        #region SaveCommand

        public RelayCommand SaveCommand { get; private set; }

        private void SaveExecute()
        {
            Diet.Plan.WaterPlan.Amount = Amount;
            Diet.Plan.WaterPlan.IntakeCount = IntakeCount;
            Diet.SaveDietPlan();
            NavigationProvider.Navigate(Constants.Pages.HomePanorama);
        }

        #endregion SaveCommand

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            Amount = Diet.Plan.WaterPlan.Amount;
            IntakeCount = Diet.Plan.WaterPlan.IntakeCount;
        }
    }
}
