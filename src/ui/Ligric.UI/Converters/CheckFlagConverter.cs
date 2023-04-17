using Ligric.Core.Ligric.Core.Types.Api;
using Utils.Extensions;

namespace Ligric.UI.Converters
{
	public class CheckFlagConverter : IValueConverter
	{
		public object Convert(object currentValueObject, Type targetType, object neededObject, string language)
		{
			var hasFlags = EnumExtensions.HasFlag<ApiPermissions>(
				System.Convert.ToInt32(currentValueObject),
				System.Convert.ToInt32(neededObject));
			return hasFlags;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
