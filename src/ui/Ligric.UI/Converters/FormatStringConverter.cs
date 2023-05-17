namespace Ligric.UI.Converters
{
	public class FormatStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (parameter == null)
				return value;

			if (value is decimal valueDecimal)
			{
				return string.Format((string)parameter, valueDecimal);
			}
			else if (value is byte valueByte)
			{
				return string.Format((string)parameter, valueByte);
			}
			else if (value is string valueString && decimal.TryParse(valueString, out valueDecimal))
			{
				return string.Format((string)parameter, valueDecimal);
			}
			return "Nan";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
