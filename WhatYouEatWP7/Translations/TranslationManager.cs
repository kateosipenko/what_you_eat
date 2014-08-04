using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using ViewModels.Helpers;

namespace WhatYouEatWP7.Translations
{
    public class TranslationManager
    {
        #region Fields

        private const string TranslationsPath = "Translations/{0}.xml";

        private static TranslationManager instance= new TranslationManager();
        private CultureInfo currentCulture;
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

        public void Initialize()
        {
            SettingsManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        private void SetCurrentCulture(CultureInfo culture)
        {
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
                        translations.Add(element.Name.LocalName, element.Value);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: implement exception logging
                }
            };
            worker.RunWorkerAsync();
        }

        private void OnLanguageChanged(object sender, LanguageEventArgs args)
        {
            SetCurrentCulture(new CultureInfo(args.NewLanguage.CultureCode));
        }
    }
}
