using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class Locator
    {
        #region Home

        private static HomeViewModel home;

        public static HomeViewModel HomeStatic
        {
            get
            {
                if (home == null)
                {
                    home = new HomeViewModel();
                }

                return home;
            }
        }

        public HomeViewModel Home
        {
            get
            {
                return HomeStatic;
            }
        }

        public static void CleanHome()
        {
            if (home != null)
            {
                home.Dispose();
                home = null;
            }
        }

        #endregion Home

        #region UserData

        private static UserDataViewModel userData;

        public static UserDataViewModel UserDataStatic
        {
            get
            {
                if (userData == null)
                {
                    userData = new UserDataViewModel();
                }

                return userData;
            }
        }

        public UserDataViewModel UserData
        {
            get { return UserDataStatic; }
        }

        public void CleanUserData()
        {
            if (userData != null)
            {
                userData.Dispose();
                userData = null;
            }
        }

        #endregion UserData

        #region Goal

        private static GoalViewModel goal;

        public static GoalViewModel GoalStatic
        {
            get
            {
                if (goal == null)
                {
                    goal = new GoalViewModel();
                }

                return goal;
            }
        }

        public GoalViewModel Goal
        {
            get { return GoalStatic; }
        }

        public void CleanGoal()
        {
            if (goal != null)
            {
                goal.Dispose();
                goal = null;
            }
        }

        #endregion Goal
        
        #region FoodSearch

        private static SearchViewModel search;

        public static SearchViewModel SearchStatic
        {
            get
            {
                if (search == null)
                {
                    search = new SearchViewModel();
                }

                return search;
            }
        }

        public SearchViewModel Search
        {
            get { return SearchStatic; }
        }

        #endregion FoodSearch

        #region FoodDetails

        private static FoodDetailsViewModel foodDetails;

        public static FoodDetailsViewModel FoodDetailsStatic
        {
            get
            {
                if (foodDetails == null)
                {
                    foodDetails = new FoodDetailsViewModel();
                }

                return foodDetails;
            }
        }

        public FoodDetailsViewModel FoodDetails
        {
            get { return FoodDetailsStatic; }
        }

        public static void CleanFoodDetails()
        {
            if (foodDetails != null)
            {
                foodDetails.Dispose();
                foodDetails = null;
            }
        }

        #endregion FoodDetails

        #region EnergyToday

        private static EnergyTodayViewModel energyToday;

        public static EnergyTodayViewModel EnergyTodayStatic
        {
            get
            {
                if (energyToday == null)
                {
                    energyToday = new EnergyTodayViewModel();
                }

                return energyToday;
            }
        }

        public EnergyTodayViewModel EnergyToday
        {
            get { return EnergyTodayStatic; }
        }

        public static void CleanEnergyToday()
        {
            if (energyToday != null)
            {
                energyToday.Dispose();
                energyToday = null;
            }
        }

        #endregion EnergyToday

        #region ActivityDetails

        private static ActivityDetailsViewModel acitivityDetails = new ActivityDetailsViewModel();

        public static ActivityDetailsViewModel ActivityDetailsStatic
        {
            get
            {
                if (acitivityDetails == null)
                {
                    acitivityDetails = new ActivityDetailsViewModel();
                }

                return acitivityDetails;
            }
        }

        public ActivityDetailsViewModel ActivityDetails
        {
            get { return ActivityDetailsStatic; }
        }

        public static void CleanActivityDetails()
        {
            if (acitivityDetails != null)
            {
                acitivityDetails.Dispose();
                acitivityDetails = null;
            }
        }

        #endregion ActivityDetails

        #region DistributeCalories

        private static DistributeCaloriesViewModel distributeCalories;

        public static DistributeCaloriesViewModel DistributeCaloriesStatic
        {
            get
            {
                if (distributeCalories == null)
                {
                    distributeCalories = new DistributeCaloriesViewModel();
                }

                return distributeCalories;
            }
        }

        public DistributeCaloriesViewModel DistributeCalories
        {
            get { return DistributeCaloriesStatic; }
        }

        #endregion DistributeCalories

        #region Settings

        private static SettingsViewModel settings;

        public static SettingsViewModel SettingsStatic
        {
            get
            {
                if (settings == null)
                    settings = new SettingsViewModel();
                return settings;
            }
        }

        public SettingsViewModel Settings
        {
            get { return SettingsStatic; }
        }

        public static void CleanSettings()
        {
            if (settings != null)
            {
                settings.Dispose();
                settings = null;
            }
        }

        #endregion Settings
    }
}
