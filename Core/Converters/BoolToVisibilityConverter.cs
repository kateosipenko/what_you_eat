using System;
using System.Windows;
using System.Windows.Data;

namespace Core.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var result = Visibility.Collapsed;
			if ((bool)value)
			{
				result = Visibility.Visible;
			}
			if (parameter != null && parameter.ToString() == "reverse")
			{
				if (result == Visibility.Visible)
				{
					result = Visibility.Collapsed;
				}
				else
				{
					result = Visibility.Visible;
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
