using System;
using System.Globalization;
using System.Windows.Data;

namespace Core.Converters
{
    public class MilesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			double miles = double.Parse(value.ToString(), CultureInfo.InvariantCulture);
            return miles + " miles";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
