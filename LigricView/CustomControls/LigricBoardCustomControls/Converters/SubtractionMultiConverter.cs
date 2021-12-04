using System;
using WinRTMultibinding.Foundation.Interfaces;

namespace LigricBoardCustomControls.Converters
{
    internal class SubtractionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, string language)
        {
            return "Kek";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
