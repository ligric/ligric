using System;
using Windows.UI.Xaml;
using WinRTMultibinding.Foundation.Data;
using WinRTMultibinding.Foundation.Interfaces;

namespace WinRTMultibinding.Foundation.PropertyDescriptors
{
    internal abstract class DependencyPropertyDescriptorBase : IDependencyPropertyDescriptor
    {
        public DependencyProperty DependencyProperty { get; }

        public abstract Type PropertyType { get; }


        protected DependencyPropertyDescriptorBase(Type propertyOwnerType, string propertyName)
        {
            DependencyProperty = propertyOwnerType.ExtractDependencyProperty(propertyName);
        }


        public object GetValue(DependencyObject dependencyObject)
            => dependencyObject.GetValue(DependencyProperty);

        public void SetValue(DependencyObject dependencyObject, object value)
            => dependencyObject.SetValue(DependencyProperty, value);
    }
}