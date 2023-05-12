namespace Ligric.UI.Converters
{
	public class NullToEmptyStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language) => value ?? "";
		public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
	}
}
