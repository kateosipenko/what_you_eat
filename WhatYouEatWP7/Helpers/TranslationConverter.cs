using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ViewModels.Helpers;

namespace WhatYouEatWP7.Helpers
{
    public class TranslationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = string.Empty;
            if (value != null && value is string)
            {
                string stringValue = (string)value;
                if (!string.IsNullOrEmpty(stringValue))
                {
                    if (TranslationManager.Instance.FoodTranslations.ContainsKey(stringValue))
                        result = TranslationManager.Instance.FoodTranslations[stringValue];

                    if (TranslationManager.Instance.ActivityTranslations.ContainsKey(stringValue))
                        result = TranslationManager.Instance.ActivityTranslations[stringValue];
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
