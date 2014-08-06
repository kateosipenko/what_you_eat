using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ViewModels.Helpers;

namespace ViewModels
{
    public class FoodSearchViewModel : ViewModel
    {
        #region Fields

        private ObservableCollection<Food> searchResults = new ObservableCollection<Food>();
        private string query = string.Empty;

        #endregion Fields

        public FoodSearchViewModel()
        {
            this.NavigateToFoodDetailsCommand = new RelayCommand<Food>(NavigateToFoodDetailsExecute);
        }

        #region Properties

        public string Query
        {
            get { return query; }
            set
            {
                if (query != value)
                {
                    query = value;
                    if (string.IsNullOrWhiteSpace(query))
                    {
                        SearchResults.Clear();
                    }
                    else
                    {
                        Search();
                    }

                    RaisePropertyChanged("Query");
                }                
            }
        }

        public ObservableCollection<Food> SearchResults
        {
            get { return searchResults; }
            set
            {
                searchResults = value;
                RaisePropertyChanged("SearchResults");
            }
        }

        #endregion Properties

        #region NavigateToFoodDetails

        public RelayCommand<Food> NavigateToFoodDetailsCommand { get; private set; }

        private void NavigateToFoodDetailsExecute(Food food)
        {
            NavigationProvider.NavigateAndRemoveBackEntry(Constants.Pages.FoodDetails.AddPageParameter(Constants.NavigationParameters.FoodId, food.Id));
        }

        #endregion NavigateToFoodDetails

        private void Search()
        {
            BusyCount++;
            RunInBackground(() =>
            {
                var result = TranslationManager.Instance.SearchFood(query);
                InvokeInUIThread(() =>
                {
                    BusyCount--;
                    SearchResults = new ObservableCollection<Food>(result);                    
                });
            });
        }
    }
}