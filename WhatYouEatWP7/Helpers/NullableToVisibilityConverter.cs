using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;

namespace WhatYouEatWP7.Helpers
{
    /// <summary>
    /// If value is null returns Visibility.Collapsed. If value is Icollection type and count of ellements is 0, returns Collapsed.
    /// </summary>
    public class NullableToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility result = value != null ? Visibility.Visible : Visibility.Collapsed;
            if (value != null)
            {
                if (value is string && string.IsNullOrEmpty((string)value))
                    result = Visibility.Collapsed;
                if (value is int && (int)value == 0)
                    result = Visibility.Collapsed;
                if (value is ICollection && ((ICollection)value).Count == 0 )
                    result = Visibility.Collapsed;
            }

            if (parameter != null && ((string)parameter) == "invert")
                result = result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
