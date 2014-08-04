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
    }
}
