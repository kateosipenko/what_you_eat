using DataAccess.Repositories;
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private ObservableCollection<Food> foods = new ObservableCollection<Food>();

        public HomeViewModel()
        {
            InitializeViewModelCommand = new RelayCommand(InitializeViewModelExecute);
        }

        public ObservableCollection<Food> Food
        {
            get { return foods; }
            set
            {
                foods = value;
                RaisePropertyChanged("Food");
            }
        }

        protected override void InitializeViewModelExecute()
        {
            base.InitializeViewModelExecute();
            RunInBackground(() =>
                {
                    List<Food> loadedFood = new List<Food>();
                    using (var foodRepo = new FoodRepository())
                    {
                        loadedFood = foodRepo.GetAllFoods();
                    }

                    InvokeInUIThread(() => Food = new ObservableCollection<Food>(loadedFood));
                });
        }
    }
}
