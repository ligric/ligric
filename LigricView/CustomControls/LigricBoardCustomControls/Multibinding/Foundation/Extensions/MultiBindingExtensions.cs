using Windows.UI.Xaml;
using WinRTMultibinding.Foundation.Data;

namespace WinRTMultibinding.Foundation.Extensions
{
    public static class MultiBindingExtensions
    {
        public static MultiBindingExpression GetMultiBindingExpression(this FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
            => MultiBindingHelper.GetMultiBindingFor(frameworkElement, dependencyProperty)?.GetExpression();
    }
}