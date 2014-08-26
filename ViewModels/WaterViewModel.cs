using Coding4Fun.Toolkit.Controls;
using GalaSoft.MvvmLight.Command;
using Resources.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModels.Helpers;

namespace ViewModels
{
    public class WaterViewModel : ViewModel
    {
        public WaterViewModel()
        {
            DrinkCommand = new RelayCommand(DrinkExecute);
        }

        #region WaterToday

        private int waterToday = 0;

        public int WaterToday
        {
            get { return waterToday; }
            set
            {
                waterToday = value;
                RaisePropertyChanged("WaterToday");
            }
        }

        #endregion WaterToday

        #region DrinkCommand

        public RelayCommand DrinkCommand { get; private set; }

        private void DrinkExecute()
        {
            InputPrompt prompt = new InputPrompt();
            prompt.Message = CommonStrings.AmountOfWater;
            prompt.Completed += (sender, args) =>
            {
                int result;
                int.TryParse(prompt.Value, out result);
                if (result > 0)
                {
                    Diet.DrinkWater(result);
                    InvokeInUIThread(() => WaterToday = Diet.WaterToday);
                }
            };

            prompt.Show();
        }

        #endregion DrinkCommand

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            WaterToday = Diet.WaterToday;
        }
    }
}
