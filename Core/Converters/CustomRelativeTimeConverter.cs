using System;
using System.Windows.Data;

namespace Core.Converters
{
	public class CustomRelativeTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var result = string.Empty;
			if (value != null)
			{
				var date = (DateTime)value;
				var difference = DateTime.Now - date;
			}
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
