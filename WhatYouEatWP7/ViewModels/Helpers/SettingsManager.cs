using IsolatedStorageHelper;
using Models;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ViewModels.Helpers
{
    public class SettingsManager : RaisableObject
    {
        #region Fields

        private static SettingsManager instance = new SettingsManager();

        private Language currentLanguage;
        private SynchronizationContext syncContext;
        private List<Language> allLanguages = new List<Language>();

        #endregion Fields

        private SettingsManager()
        {
            allLanguages = new List<Language>();
            allLanguages.Add(new Language { CultureCode = Constants.Languages.EnglishCode, Text = Constants.Languages.English });
            allLanguages.Add(new Language { CultureCode = Constants.Languages.UkrainianCode, Text = Constants.Languages.Ukrainian });
            allLanguages.Add(new Language { CultureCode = Constants.Languages.RussianCode, Text = Constants.Languages.Russian });
        }

        #region Properties

        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsManager();
                }

                return instance;
            }
        }

        public List<Language> AllLanguages
        {
            get { return allLanguages; }
        }

        public Language CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                RaiseLanguageChanged(currentLanguage, value);
                currentLanguage = value;
                RaisePropertyChanged("CurrentLanguage");                
            }
        }

        #endregion Properties

        #region Initialization

        public void Initialize()
        {
            if (syncContext == null)
            {
                SynchronizationContextProvider.Initialize();
                syncContext = SynchronizationContextProvider.UIThreadSyncContext;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                currentLanguage = IsolatedStorage.ReadValue<Language>(Constants.CacheKeys.CurrentLanguage);
                if (currentLanguage == null)
                {
                    currentLanguage = allLanguages.First();
                }

                syncContext.Post((item) =>
                {
                    CurrentLanguage = currentLanguage;
                }, null);
            };
            worker.RunWorkerAsync();
        }

        #endregion Initialization

        #region LanguageChanged

        public event EventHandler<LanguageEventArgs> LanguageChanged;

        private void RaiseLanguageChanged(Language old, Language newLang)
        {
            if (LanguageChanged != null)
            {
                LanguageChanged(this, new LanguageEventArgs(old, newLang));
            }
        }

        #endregion LanguageChanged
    }

    public class LanguageEventArgs : EventArgs
    {
        private Language oldLanguage;
        private Language newLanguage;

        public LanguageEventArgs(Language oldLanguage, Language newLanguage)
        {
            this.oldLanguage = oldLanguage;
            this.newLanguage = newLanguage;
        }

        public Language OldLanguage
        {
            get { return oldLanguage; }
        }

        public Language NewLanguage
        {
            get { return newLanguage; }
        }
    }
}
