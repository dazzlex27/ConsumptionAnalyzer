using System;
using System.Globalization;
using System.Windows.Data;

namespace ConsumptionAnalyzerTestApp
{
	internal class StringToEnabledConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var str = value as string;
			if (str == null)
				return null;

			return !string.IsNullOrEmpty(str);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
