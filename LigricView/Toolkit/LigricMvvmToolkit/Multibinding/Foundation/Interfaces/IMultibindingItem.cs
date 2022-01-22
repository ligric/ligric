using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LigricMvvmToolkit.Multibinding.Foundation.Interfaces
{
    internal interface IMultibindingItem
    {
        BindingMode Mode { get; }


        void Initialize(FrameworkElement targetElement);
    }
}