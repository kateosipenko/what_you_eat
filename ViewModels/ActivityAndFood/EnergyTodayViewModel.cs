﻿using DataAccess.Tables;
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
    public class EnergyTodayViewModel : ViewModel
    {
        private EnergyType energyType = EnergyType.None;

        public EnergyTodayViewModel()
        {
            DeleteFromTodayCommand = new RelayCommand<RaisableObject>(DeleteFromTodayExecute);
            NavigateToSearchCommand = new RelayCommand(NavigateToSearchExecute);
        }

        #region Initialization

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            BusyCount++;
            var parameters = NavigationProvider.GetNavigationParameters();
            if (parameters.ContainsKey(Constants.NavigationParameters.EnergyType))
            {
                energyType = (EnergyType)Enum.Parse(typeof(EnergyType), parameters[Constants.NavigationParameters.EnergyType], true);
                LoadDataForToday();
            }
        }

        private void LoadDataForToday()
        {
            RunInBackground(() =>
            {
                List<RaisableObject> energyForToday = new List<RaisableObject>();
                switch (energyType)
                {
                    case EnergyType.Food:
                        energyForToday = Diet.GetEatenToday().Cast<RaisableObject>().ToList();
                        break;
                    case EnergyType.Activity:
                        energyForToday = Diet.GetSpentToday().Cast<RaisableObject>().ToList();
                        break;
                }

                InvokeInUIThread(() =>
                {
                    TodayItems = new ObservableCollection<RaisableObject>(energyForToday);
                    UpdateTotalSpent();
                    BusyCount--;
                });
            });
        }

        #endregion Initialization

        #region TotalEnergy

        private int totalEnergy = 0;

        public int TotalEnergy
        {
            get { return totalEnergy; }
            set
            {
                totalEnergy = value;
                RaisePropertyChanged("TotalEnergy");
            }
        }

        private void UpdateTotalSpent()
        {
            switch (energyType)
            {
                case EnergyType.Activity:
                    TotalEnergy = TodayItems.Sum(item => ((Exersize)item).CaloriesSpent);
                    break;
                case EnergyType.Food:
                    TotalEnergy = TodayItems.Sum(item => ((Food)item).AmountOfCalories);
                    break;
            }
        }

        #endregion TotalEnergy

        #region TodayItems

        private ObservableCollection<RaisableObject> todayItems = new ObservableCollection<RaisableObject>();

        public ObservableCollection<RaisableObject> TodayItems
        {
            get { return todayItems; }
            set
            {
                todayItems = value;
                RaisePropertyChanged("TodayItems");
            }
        }

        #endregion TodayItems

        #region AddOrDelete

        public void AddEnergy(RaisableObject energy)
        {
            RaisableObject newEnergy = energy;
            switch (energyType)
            {
                case EnergyType.Food:
                    newEnergy = Diet.EatFood((Food)energy);
                    break;
                case EnergyType.Activity:
                    newEnergy = Diet.DoExersize((Exersize)energy);
                    break;
            }

            TodayItems.Add(newEnergy);
            UpdateTotalSpent();
            BusyCount--;
        }

        public RelayCommand<RaisableObject> DeleteFromTodayCommand { get; private set; }

        private void DeleteFromTodayExecute(RaisableObject energy)
        {
            BusyCount++;
            RunInBackground(() =>
            {
                switch (energyType)
                {
                    case EnergyType.Activity:
                        Diet.DeleteExersize((Exersize)energy);
                        break;
                    case EnergyType.Food:
                        Diet.DeleteEatenFood((Food)energy);
                        break;
                }

                InvokeInUIThread(() =>
                {
                    TodayItems.Remove(energy);
                    UpdateTotalSpent();
                    BusyCount--;
                });
            });
        }

        #endregion AddOrDelete

        #region NavigateToSearchCommand

        public RelayCommand NavigateToSearchCommand { get; private set; }

        private void NavigateToSearchExecute()
        {
            NavigationProvider.Navigate(Constants.Pages.Search.AddPageParameter(Constants.NavigationParameters.EnergyType, energyType));
        }

        #endregion NavigateToSearchCommand

        #region Clear

        protected override void CleanupExecute()
        {
            Clear();
            base.CleanupExecute();
        }

        private void Clear()
        {
            TotalEnergy = 0;
            TodayItems.Clear();
            energyType = EnergyType.None;
        }

        #endregion Clear
    }
}
