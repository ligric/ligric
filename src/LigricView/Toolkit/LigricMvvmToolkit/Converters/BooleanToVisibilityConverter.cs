using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LigricMvvmToolkit.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!Boolean.TryParse(value.ToString(), out bool boolValue))
                return value;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (!Enum.TryParse<Visibility>(value.ToString(), out Visibility visibilityValue))
                return value;

            return visibilityValue == Visibility.Visible ? true : false;
        }
    }
}
