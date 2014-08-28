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

        #region Language

        public List<Language> AllLanguages
        {
            get { return allLanguages; }
        }

        public Language CurrentLanguage
        {
            get { return currentLanguage; }
        }

        public void SetCurrentLanguage(Language language)
        {
            if (currentLanguage != language)
            {
                RaiseLanguageChanged(currentLanguage, language);
                currentLanguage = language;
                RaisePropertyChanged("CurrentLanguage");
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (sender, args) =>
                {
                    CacheManager.Instance.SaveCurrentLanguage(currentLanguage);
                };
                worker.RunWorkerAsync();
            }

        }

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

        #endregion Language

        #region Initialization

        public void Initialize()
        {
            if (syncContext == null)
            {
                syncContext = SynchronizationContextProvider.UIThreadSyncContext;
            }

            var currentCultureCode = System.Globalization.CultureInfo.CurrentUICulture.Name;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                Language language = CacheManager.Instance.GetCurrentLanguage();
                if (language == null)
                {
                    var deviceLanguage = allLanguages.FirstOrDefault(item => item.CultureCode == currentCultureCode);
                    if (deviceLanguage != null)
                    {
                        language = deviceLanguage;
                    }
                    else
                    {
                        // TODO: propose to select the language from the list
                        language = allLanguages.First();
                    }
                }

                syncContext.Post((item) =>
                {
                    SetCurrentLanguage(language);
                }, null);
            };
            worker.RunWorkerAsync();
        }

        #endregion Initialization
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
