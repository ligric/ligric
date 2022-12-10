using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LigricMvvmToolkit.Extensions
{
    public static class FrameworkElementHelper
    {
        public static bool TransformInitialize(this FrameworkElement element)
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

        public static TranslateTransform GetTransformInitialize(this FrameworkElement element)
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
