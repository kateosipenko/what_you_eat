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
    public class FoodDetailsViewModel : ViewModel
    {
        #region CurrentFood

        private Food currentFood;

        public Food CurrentFood
        {
            get { return currentFood; }
            set
            {
                currentFood = value;
                RaisePropertyChanged("CurrentFood");
            }
        }

        #endregion CurrentFood

        public FoodDetailsViewModel()
        {
            InitializeCommand = new RelayCommand(InitializeExecute);
            EatFoodCommand = new RelayCommand(EatFoodExecute);
        }

        #region Initialization

        protected override void InitializeExecute()
        {
            BusyCount++;
            base.InitializeExecute();
            var parameters = NavigationProvider.GetNavigationParameters();
            if (parameters.ContainsKey(Constants.NavigationParameters.FoodId))
            {
                RunInBackground(() =>
                {
                    Food result;
                    using (var repo = new FoodRepository())
                    {
                        result = repo.GetById(int.Parse(parameters[Constants.NavigationParameters.FoodId]));
                    }

                    InvokeInUIThread(() =>
                    {
                        CurrentFood = result;
                        BusyCount--;
                    });
                });
            }
        }

        #endregion Initialization

        #region EatFoodCommand

        public RelayCommand EatFoodCommand { get; private set; }

        private void EatFoodExecute()
        {
            Locator.EnergyTodayStatic.AddEnergy(CurrentFood);
            if (NavigationProvider.CanGoBack())
                NavigationProvider.GoBack();
        }

        #endregion EatFoodCommand

        #region Cleanup

        public override void Cleanup()
        {
            base.Cleanup();
            this.CurrentFood = null;
        }

        #endregion Cleanup
    }
}
