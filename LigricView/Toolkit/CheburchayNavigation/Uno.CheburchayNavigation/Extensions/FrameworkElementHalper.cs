using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Uno.CheburchayNavigation.Extensions
{
    public static class FrameworkElementHalper
    {
        internal static bool TransformInitialize(this FrameworkElement element)
        {
            if (element is null)
                return false;

            var renderTransform = element.RenderTransform as TranslateTransform;

            if (renderTransform is null)
            {
                renderTransform = new TranslateTransform();
                element.RenderTransform = renderTransform;
            }
            return true;
        }

        internal static TranslateTransform GetTransformInitialize(this FrameworkElement element)
        {
            if (element is null)
                return null;

            var renderTransform = element.RenderTransform as TranslateTransform;

            if (renderTransform is null)
            {
                renderTransform = new TranslateTransform();
                element.RenderTransform = renderTransform;
            }
            return renderTransform;
        }
    }
}
