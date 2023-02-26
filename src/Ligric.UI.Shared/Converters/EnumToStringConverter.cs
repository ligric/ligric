namespace Ligric.UI.Converters
{
	public class EnumToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return value?.ToString() ?? DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
