using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Core.Converters
{
    public class MinutesToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int totalMinutes = (int)value;
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

			var resultBuilder = new StringBuilder();
			if(hours < 10)
			{
				resultBuilder.Append("0");
			}
			resultBuilder.Append(hours);
			resultBuilder.Append(":");
			if (minutes < 10)
			{
				resultBuilder.Append("0");
			}
			resultBuilder.Append(minutes);

			return resultBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
