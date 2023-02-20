namespace Ligric.UI.Converters
{
	public class EnumToIntConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null || !int.TryParse(value.ToString(), out int result))
			{
				return DependencyProperty.UnsetValue;
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			string? valueString = value.ToString();
			if (valueString == null)
			{
				return DependencyProperty.UnsetValue;
			}

			return Enum.Parse(targetType, valueString);
		}
	}
}
