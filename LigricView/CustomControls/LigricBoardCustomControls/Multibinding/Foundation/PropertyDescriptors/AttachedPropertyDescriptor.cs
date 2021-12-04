using System;
using System.Reflection;

namespace WinRTMultibinding.Foundation.PropertyDescriptors
{
    internal class AttachedPropertyDescriptor : DependencyPropertyDescriptorBase
    {
        private const string Get = "Get";
        private const string Set = "Set";


        public override Type PropertyType { get; }


        public AttachedPropertyDescriptor(Type attachedPropertyOwnerType, string propertyName)
            : base(attachedPropertyOwnerType, propertyName)
        {
            var attachedPropertyOwnerTypeInfo = attachedPropertyOwnerType.GetTypeInfo();
            var name = $"{attachedPropertyOwnerType.Name}.{propertyName}";
            var getMethod = attachedPropertyOwnerTypeInfo.GetDeclaredMethod(Get + propertyName);
            var setMethod = attachedPropertyOwnerTypeInfo.GetDeclaredMethod(Set + propertyName);

            if (getMethod != null)
            {
                PropertyType = getMethod.ReturnType;
            }
            else if (setMethod != null)
            {
                var parameters = setMethod.GetParameters();
                PropertyType = parameters[1].ParameterType;
            }
            else
            {
                throw new InvalidOperationException($"Attached property {name} doesn't have neither get method nor set method.");
            }
        }
    }
}