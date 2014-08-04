using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using WhatYouEatWP7.Resources.Common;

namespace WhatYouEatWP7.Helpers
{
    public class CourseToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return CommonStrings.ResourceManager.GetString(Enum.GetName(typeof(Course), value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
