using Core.Helpers;
using DataAccess.Repositories;
using DataAccess.Tables;
using IsolatedStorageHelper;
using Models;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ViewModels.Helpers
{
    public class CacheManager
    {
        #region Singleton

        private static CacheManager instance = new CacheManager();

        private CacheManager()
        {
        }

        public static CacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CacheManager();
                }

                return instance;
            }
        }

        #endregion Singleton

        #region Language

        public void SaveCurrentLanguage(Language language)
        {
            IsolatedStorage.WriteValue(Constants.CacheKeys.Language, language);
        }

        public Language GetCurrentLanguage()
        {
            return IsolatedStorage.ReadValue<Language>(Constants.CacheKeys.Language);
        }

        #endregion Language
    }
}
