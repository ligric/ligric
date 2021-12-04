using System;
using Windows.UI.Xaml;

namespace WinRTMultibinding.Foundation.Interfaces
{
    internal interface IDependencyPropertyDescriptor
    {
        DependencyProperty DependencyProperty { get; }

        Type PropertyType { get; }


        object GetValue(DependencyObject dependencyObject);

        void SetValue(DependencyObject dependencyObject, object value);
    }
}