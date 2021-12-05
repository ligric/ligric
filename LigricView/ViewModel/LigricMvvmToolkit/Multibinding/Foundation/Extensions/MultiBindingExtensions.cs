using Windows.UI.Xaml;
using LigricMvvmToolkit.Multibinding.Foundation.Data;

namespace LigricMvvmToolkit.Multibinding.Foundation.Extensions
{
    public static class MultiBindingExtensions
    {
        public static MultiBindingExpression GetMultiBindingExpression(this FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
            => MultiBindingHelper.GetMultiBindingFor(frameworkElement, dependencyProperty)?.GetExpression();
    }
}