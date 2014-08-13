using Core.Helpers;
using DataAccess.Repositories;
using DataAccess.Tables;
using Models;
using Resources;
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

        private const string FoodTranslationsPath = "FoodTranslations/{0}.xml";
        private const string ActivityTranslationsPath = "ActivityTranslations/{0}.xml";

        private static TranslationManager instance= new TranslationManager();
        private CultureInfo currentCulture;
        private SynchronizationContext syncContext;
        private Dictionary<string, string> foodTranslations = new Dictionary<string, string>();
        private Dictionary<string, string> activityTranslations = new Dictionary<string, string>();

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

        public Dictionary<string, string> FoodTranslations
        {
            get { return foodTranslations; }
        }

        public Dictionary<string, string> ActivityTranslations
        {
            get { return activityTranslations; }
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
            ((LocalizationManager)Application.Current.Resources["LocalizationManager"]).RefreshLanguage();

            if (currentCulture != null)
            {
                activityTranslations.Clear();
                foodTranslations.Clear();
            }

            currentCulture = culture;
            LoadTranslations();
        }

        private void LoadTranslations()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                LoadStringsFromFile(FoodTranslationsPath, foodTranslations);
                LoadStringsFromFile(ActivityTranslationsPath, activityTranslations);
            };
            worker.RunWorkerAsync();
        }

        private void LoadStringsFromFile(string path, Dictionary<string, string> target)
        {
            try
            {
                var root = XElement.Load(string.Format(path, currentCulture.Name.ToLower()));
                var elements = root.Elements();
                foreach (var element in elements)
                {
                    if (!target.ContainsKey(element.Name.LocalName))
                    {
                        target.Add(element.Name.LocalName, element.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex);
            }
        }

        private void OnLanguageChanged(object sender, LanguageEventArgs args)
        {
            var newCulture = new CultureInfo(args.NewLanguage.CultureCode);
            SetCurrentCulture(newCulture);
        }

        #endregion Laguage Setup

        #region Search

        public List<RaisableObject> Search(string query, EnergyType type)
        {
            query = query.ToLower();
            List<RaisableObject> result = new List<RaisableObject>();
            switch (type)
            {
                case EnergyType.Activity:
                    var activityKeys = activityTranslations.Where(item => item.Value.ToLower().StartsWith(query)).Select(item => item.Key);
                    using (var repo = new PhysicalActivityRepository())
                    {
                        result = repo.Search(activityKeys).Cast<RaisableObject>().ToList();
                    }

                    break;
                case EnergyType.Food:
                    var foodKeys = this.foodTranslations.Where(item => item.Value.ToLower().StartsWith(query)).Select(item => item.Key);
                    using (var repo = new FoodRepository())
                    {
                        result = repo.Search(foodKeys).Cast<RaisableObject>().ToList();
                    }
                    break;
            }

            return result;
        }

        #endregion Search
    }
}
