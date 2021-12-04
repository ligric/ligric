using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTMultibinding.Foundation.Interfaces
{
    internal interface IMultibindingItem
    {
        BindingMode Mode { get; }


        void Initialize(FrameworkElement targetElement);
    }
}