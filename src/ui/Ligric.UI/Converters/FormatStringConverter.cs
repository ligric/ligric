namespace Ligric.UI.Converters
{
	public class FormatStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (parameter == null)
				return value;

			decimal? valueDecimal = value as decimal?;

			if (valueDecimal == null)
			{
				byte? valueByte = value as byte?;
				if (valueByte != null)
				{
					return string.Format((string)parameter, valueByte);
				}
				return "Nan";
			}

			return string.Format((string)parameter, valueDecimal);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
