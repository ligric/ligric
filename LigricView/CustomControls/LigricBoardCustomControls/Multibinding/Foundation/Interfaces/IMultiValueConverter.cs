using System;

namespace WinRTMultibinding.Foundation.Interfaces
{
    public interface IMultiValueConverter
    {
        object Convert(object[] values, Type targetType, object parameter, string language);

        object[] ConvertBack(object value, Type[] targetTypes, object parameter, string language);
    }
}