using DataAccess.Repositories;
using DataAccess.Tables;
using IsolatedStorageHelper;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModels.Helpers
{
    public class UserManager
    {
        #region Fields

        private static UserManager instance = new UserManager();

        private DateTime birthday;
        private BodyState currentState;

        #endregion Fields

        private UserManager() { }

        #region Properties

        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserManager();
                }

                return instance;
            }
        }

        public DateTime Birthday
        {
            get { return birthday; }
        }

        public BodyState CurrentState
        {
            get { return currentState; }
        }

        #endregion Properties

        public bool IsFirstStart()
        {
            using (BodyStateRepository repo = new BodyStateRepository())
            {
                currentState = repo.GetLastState();
            }

            birthday = IsolatedStorage.ReadValue<DateTime>(Constants.CacheKeys.Birthday);
            return currentState == null || birthday == default(DateTime);
        }
    }
}
