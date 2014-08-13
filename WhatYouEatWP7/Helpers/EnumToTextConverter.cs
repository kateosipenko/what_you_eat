using Models;
using Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WhatYouEatWP7.Helpers
{
    public class EnumToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ActivityType)
                return EnumsStrings.ResourceManager.GetString(((ActivityType)value).Key);
            else
                return EnumsStrings.ResourceManager.GetString(Enum.GetName(value.GetType(), value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
