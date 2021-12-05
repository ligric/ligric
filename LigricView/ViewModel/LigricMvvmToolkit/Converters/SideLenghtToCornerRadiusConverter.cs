using System;
using Windows.UI.Xaml.Data;

namespace LigricMvvmToolkit.Converters
{
    public class SideLenghtToCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && double.TryParse(value.ToString(), out double doubleParameter))
            {
                if (doubleParameter > 0)
                {
                    return doubleParameter / 2;
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
