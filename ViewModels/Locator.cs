using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class Locator
    {
        #region GoalPlan

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

        #region FoodPlan

        private static FoodPlanViewModel foodPlan;

        public static FoodPlanViewModel FoodPlanStatic
        {
            get
            {
                if (foodPlan == null)
                {
                    foodPlan = new FoodPlanViewModel();
                }

                return foodPlan;
            }
        }

        public FoodPlanViewModel FoodPlan
        {
            get { return FoodPlanStatic; }
        }

        #endregion FoodPlan

        #region TrainingsPlan

        private static TrainingsPlanViewModel trainingsPlan;

        public static TrainingsPlanViewModel TrainingsPlanStatic
        {
            get
            {
                if (trainingsPlan == null)
                {
                    trainingsPlan = new TrainingsPlanViewModel();
                }

                return trainingsPlan;
            }
        }

        public TrainingsPlanViewModel TrainingsPlan
        {
            get { return TrainingsPlanStatic; }
        }

        #endregion TrainingsPlan

        #region TrainingDetails

        private static TrainingDetailsViewModel trainingDetails;

        public static TrainingDetailsViewModel TrainingDetailsStatic
        {
            get
            {
                if (trainingDetails == null)
                {
                    trainingDetails = new TrainingDetailsViewModel();
                }

                return trainingDetails;
            }
        }

        public TrainingDetailsViewModel TrainingDetails
        {
            get { return TrainingDetailsStatic; }
        }

        #endregion TrainingDetails

        #region WaterPlan

        private static WaterPlanViewModel waterPlan;

        public static WaterPlanViewModel WaterPlanStatic
        {
            get
            {
                if (waterPlan == null)
                {
                    waterPlan = new WaterPlanViewModel();
                }

                return waterPlan;
            }
        }

        public WaterPlanViewModel WaterPlan
        {
            get { return WaterPlanStatic; }
        }

        #endregion WaterPlan

        #endregion GoalPlan

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

        #region ExersizeDetails

        private static ExersizeDetailsViewModel exersizeDetails = new ExersizeDetailsViewModel();

        public static ExersizeDetailsViewModel ExersizeDetailsStatic
        {
            get
            {
                if (exersizeDetails == null)
                {
                    exersizeDetails = new ExersizeDetailsViewModel();
                }

                return exersizeDetails;
            }
        }

        public ExersizeDetailsViewModel ExersizeDetails
        {
            get { return ExersizeDetailsStatic; }
        }

        public static void CleanExersizeDetails()
        {
            if (exersizeDetails != null)
            {
                exersizeDetails.Dispose();
                exersizeDetails = null;
            }
        }

        #endregion ExersizeDetails

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

        #region ProfileData

        #region Profile

        private static ProfileViewModel profile;

        public static ProfileViewModel ProfileStatic
        {
            get
            {
                if (profile == null)
                    profile = new ProfileViewModel();
                return profile;
            }
        }

        public ProfileViewModel Profile
        {
            get { return ProfileStatic; }
        }

        #endregion Profile

        #region Progress

        private static ProgressViewModel progress;

        public static ProgressViewModel ProgressStatic
        {
            get
            {
                if (progress == null)
                    progress = new ProgressViewModel();

                return progress;
            }
        }

        public ProgressViewModel Progress
        {
            get { return ProgressStatic; }
        }

        #endregion Progress

        #endregion ProfileData

        #region Water

        private static WaterViewModel water;

        public static WaterViewModel WaterStatic
        {
            get
            {
                if (water == null)
                {
                    water = new WaterViewModel();
                }

                return water;
            }
        }

        public WaterViewModel Water
        {
            get { return WaterStatic; }
        }

        public void CleanWater()
        {
            if (water != null)
            {
                water.Dispose();
                water = null;
            }
        }

        #endregion Water

    }
}
