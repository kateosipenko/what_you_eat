using Core.Helpers;
using DataAccess.Repositories;
using DataAccess.Tables;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using ViewModels.Helpers;

namespace ViewModels.Helpers
{
    public class TranslationManager
    {
        #region Fields

        private const string TranslationsPath = "Translations/{0}.xml";

        private static TranslationManager instance= new TranslationManager();
        private CultureInfo currentCulture;
        private SynchronizationContext syncContext;
        private Dictionary<string, string> translations = new Dictionary<string, string>();

        #endregion Fields

        private TranslationManager()
        {
            
        }

        #region Properties

        public static TranslationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TranslationManager();
                }

                return instance;
            }
        }

        public CultureInfo CurrentCulture
        {
            get { return currentCulture; }
        }

        public Dictionary<string, string> Translations
        {
            get { return translations; }
        }

        #endregion Properties

        #region Laguage Setup

        public void Initialize()
        {
            SettingsManager.Instance.LanguageChanged += OnLanguageChanged;
            syncContext = SynchronizationContextProvider.UIThreadSyncContext;
        }

        private void SetCurrentCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            ((ILocalized)Application.Current.Resources["LocalizationManager"]).RefreshLanguage();

            if (currentCulture != null)
            {
                translations.Clear();
            }

            currentCulture = culture;
            LoadTranslations();
        }

        private void LoadTranslations()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                try
                {
                    var root = XElement.Load(string.Format(TranslationsPath, currentCulture.Name.ToLower()));
                    var elements = root.Elements();
                    foreach (var element in elements)
                    {
                        if (!translations.ContainsKey(element.Name.LocalName))
                        {
                            translations.Add(element.Name.LocalName, element.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogException(ex);
                }
            };
            worker.RunWorkerAsync();
        }

        private void OnLanguageChanged(object sender, LanguageEventArgs args)
        {
            var newCulture = new CultureInfo(args.NewLanguage.CultureCode);
            SetCurrentCulture(newCulture);
        }

        #endregion Laguage Setup

        #region FoodSearch

        public List<Food> SearchFood(string query)
        {
            var keys = this.translations.Where(item => item.Value.ToLower().StartsWith(query)).Select(item => item.Key);
            List<Food> result = new List<Food>();
            using (var repo = new FoodRepository())
            {
                result = repo.Search(keys);
            }

            return result;
        }

        #endregion FoodSearch
    }
}
