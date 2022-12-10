using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LigricMvvmToolkit.Converters
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!long.TryParse(value.ToString(), out long longValue))
                return value;

            return longValue > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
