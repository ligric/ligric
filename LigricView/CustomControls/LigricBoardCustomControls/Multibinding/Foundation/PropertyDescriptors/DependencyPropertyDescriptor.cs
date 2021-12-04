using System;
using System.Reflection;

namespace WinRTMultibinding.Foundation.PropertyDescriptors
{
    internal class DependencyPropertyDescriptor : DependencyPropertyDescriptorBase
    {
        public override Type PropertyType { get; }


        public DependencyPropertyDescriptor(Type frameworkElementType, string propertyName)
            : base(frameworkElementType, propertyName)
        {
            var targetPropertyInfo = frameworkElementType.GetRuntimeProperty(propertyName);
            PropertyType = targetPropertyInfo.PropertyType;
        }
    }
}