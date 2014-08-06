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

        #region Eaten

        private static EatenViewModel eaten;

        public static EatenViewModel EatenStatic
        {
            get
            {
                if (eaten == null)
                    eaten = new EatenViewModel();

                return eaten;
            }
        }

        public EatenViewModel Eaten
        {
            get { return EatenStatic; }
        }

        public void CleanEaten()
        {
            if (eaten != null)
            {
                eaten.Dispose();
                eaten = null;
            }
        }

        #endregion Eaten

        #region FoodSearch

        private static FoodSearchViewModel foodSearch;

        public static FoodSearchViewModel FoodSearchStatic
        {
            get
            {
                if (foodSearch == null)
                {
                    foodSearch = new FoodSearchViewModel();
                }

                return foodSearch;
            }
        }

        public FoodSearchViewModel FoodSearch
        {
            get { return FoodSearchStatic; }
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
    }
}
