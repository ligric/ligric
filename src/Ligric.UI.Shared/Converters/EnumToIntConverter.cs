namespace Ligric.UI.Converters
{
	public class EnumToIntConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (int)value;
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
