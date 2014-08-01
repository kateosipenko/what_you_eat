using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels
{
    public class Locator
    {
        #region Fields

        private static HomeViewModel home;

        #endregion Fields

        #region StaticProperties

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

        #endregion StaticProperties

        #region Properties

        public HomeViewModel Home
        {
            get
            {
                return HomeStatic;
            }
        }

        #endregion Properties

        #region Cleaning

        public static void CleanHome()
        {
            if (home != null)
            {
                home.Dispose();
                home = null;
            }
        }

        #endregion Cleaning

    }
}
