using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace WhatYouEatWP7.Translations
{
    public class TranslationManager
    {
        private const string TranslationsPath = "Translations/{0}.xml";

        private static TranslationManager instance= new TranslationManager();
        private CultureInfo currentCulture;
        private Dictionary<string, string> translations = new Dictionary<string, string>();

        private TranslationManager()
        {
        }

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

        public static void SetCurrentCulture(CultureInfo culture)
        {
            if (instance.currentCulture != null)
            {
                instance.translations.Clear();
            }

            instance.currentCulture = culture;
            instance.LoadTranslations();
        }

        private void LoadTranslations()
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
        }
    }
}
