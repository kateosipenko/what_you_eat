using DataAccess.Repositories;
using DataAccess.Tables;
using GalaSoft.MvvmLight.Command;
using Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ViewModels.Helpers;

namespace ViewModels
{
    public class SearchViewModel : ViewModel
    {
        #region Fields

        private ObservableCollection<RaisableObject> searchResults = new ObservableCollection<RaisableObject>();
        private string query = string.Empty;
        private EnergyType searchType = EnergyType.None;

        #endregion Fields

        public SearchViewModel()
        {
            this.InitializeCommand = new RelayCommand(InitializeExecute);
            this.CleanupCommand = new RelayCommand(CleanupExecute);
            this.NavigateToDetailsCommand = new RelayCommand<RaisableObject>(NavigateToDetailsExecute);
        }

        #region Initialization

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            var parameters = NavigationProvider.GetNavigationParameters();
            if (parameters.ContainsKey(Constants.NavigationParameters.EnergyType))
            {
                searchType = (EnergyType) Enum.Parse(typeof(EnergyType), parameters[Constants.NavigationParameters.EnergyType], true);
                GetTop();
            }
        }

        #endregion Initialization

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
                        GetTop();
                    }
                    else
                    {
                        Search();
                    }

                    RaisePropertyChanged("Query");
                }
            }
        }

        public ObservableCollection<RaisableObject> SearchResults
        {
            get { return searchResults; }
            set
            {
                searchResults = value;
                RaisePropertyChanged("SearchResults");
            }
        }

        #endregion Properties

        #region NavigateToDetailsCommand

        public RelayCommand<RaisableObject> NavigateToDetailsCommand { get; private set; }

        private void NavigateToDetailsExecute(RaisableObject energy)
        {
            string navigationString = string.Empty;
            switch (searchType)
            {
                case EnergyType.Food:
                    navigationString = Constants.Pages.FoodDetails.AddPageParameter(Constants.NavigationParameters.FoodId, ((Food)energy).Id);
                    break;
                case EnergyType.Activity:
                    navigationString = Constants.Pages.ActivityDetails.AddPageParameter(Constants.NavigationParameters.ActivityId, ((PhysicalActivity)energy).Id);
                    break;
            }

            if (!string.IsNullOrEmpty(navigationString))
            {
                Clear();
                NavigationProvider.NavigateAndRemoveBackEntry(navigationString);
            }
        }

        #endregion NavigateToDetailsCommand

        #region Clear

        protected override void CleanupExecute()
        {
            Clear();
            base.CleanupExecute();
        }

        private void Clear()
        {
            this.searchType = EnergyType.None;
            this.SearchResults.Clear();
            this.Query = string.Empty;
        }

        #endregion Clear

        private void Search()
        {
            RunInBackground(() =>
            {
                var result = TranslationManager.Instance.Search(query, searchType);
                InvokeInUIThread(() =>
                {
                    SearchResults = new ObservableCollection<RaisableObject>(result);
                });
            });
        }

        private void GetTop()
        {
            switch (searchType)
            {
                case EnergyType.Activity:
                    using (var activityRepo = new PhysicalActivityRepository())
                    {
                        SearchResults = new ObservableCollection<RaisableObject>(activityRepo.GetTopTwenty().Cast<RaisableObject>().ToList());
                    }
                    break;
                case EnergyType.Food:
                    using (var foodRepo = new FoodRepository())
                    {
                        SearchResults = new ObservableCollection<RaisableObject>(foodRepo.GetTopTwenty().Cast<RaisableObject>().ToList());
                    }

                    break;
            }
        }
    }
}