using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.AttachedProperties
{
    public static class ElementStartupFocus
    {
        public static DependencyProperty StartupFocusProperty = DependencyProperty.RegisterAttached("StartupFocus", typeof(bool),
            typeof(ElementStartupFocus), new PropertyMetadata(true, StartupFocusChanged));


        public static bool GetStartupFocus(UIElement element)
        {
            return (bool)element.GetValue(StartupFocusProperty);
        }

        public static void SetStartupFocus(UIElement element, bool value)
        {
            element.SetValue(StartupFocusProperty, value);
        }

        private static void StartupFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetStartupFocus(d, (bool)e.NewValue);
        }

        private static Control control_element = null;

        private static void SetStartupFocus(DependencyObject d, bool newState)
        {
            if (!newState)
            {
                if (d is Control)
                {
                    control_element = (Control)d;
                    control_element.GettingFocus += OnEmailTextBoxGettingFocus;
                }
            }
        }

        private static void OnEmailTextBoxGettingFocus(Windows.UI.Xaml.UIElement sender, Windows.UI.Xaml.Input.GettingFocusEventArgs args)
        {
            args.TryCancel();
            control_element.GettingFocus -= OnEmailTextBoxGettingFocus;
            return;
        }
    }
}
