using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using LigricMvvmToolkit.Multibinding.Common.Reflection;

namespace LigricMvvmToolkit.Multibinding.Foundation.Data
{
    internal static class BindingHelper
    {
        private const string DependencyPropertySuffix = "Property";

        private static readonly IDictionary<DependencyPropertyInfo, DependencyProperty> DependencyProperties;


        static BindingHelper()
        {
            DependencyProperties = new Dictionary<DependencyPropertyInfo, DependencyProperty>(new DependencyPropertyInfoEqualityComparer());
        }


        public static DependencyProperty ExtractDependencyProperty(this Type dependencyObjectType, string propertyNameWithoutSuffix)
        {
            var dependencyPropertyName = propertyNameWithoutSuffix + DependencyPropertySuffix;
            var dependencyPropertyInfo = new DependencyPropertyInfo(dependencyObjectType, dependencyPropertyName);
            DependencyProperty dependencyProperty;
            if (!DependencyProperties.TryGetValue(dependencyPropertyInfo, out dependencyProperty) || dependencyProperty == null)
            {
                dependencyProperty = ExtractDependenctyPropertyFromProperty(dependencyObjectType, dependencyPropertyName) ??
                                     ExtractDependencyPropertyFromField(dependencyObjectType, dependencyPropertyName);
                DependencyProperties[dependencyPropertyInfo] = dependencyProperty;
            }

            return dependencyProperty;
        }


        private static DependencyProperty ExtractDependenctyPropertyFromProperty(Type objectType, string dependencyPropertyName)
        {
            var propertyInfo = Reflector.ScanHierarchyForMember(objectType, typeInfo => typeInfo.GetDeclaredProperty(dependencyPropertyName));

            return (DependencyProperty)propertyInfo?.GetValue(null);
        }

        private static DependencyProperty ExtractDependencyPropertyFromField(Type objectType, string dependencyPropertyName)
        {
            var fieldInfo = Reflector.ScanHierarchyForMember(objectType, typeInfo => typeInfo.GetDeclaredField(dependencyPropertyName));

            return (DependencyProperty)fieldInfo?.GetValue(null);
        }



        private class DependencyPropertyInfo
        {
            public Type DependencyObjectType { get; }

            public string DependencyPropertyName { get; }


            public DependencyPropertyInfo(Type dependencyObjectType, string dependencyPropertyName)
            {
                DependencyObjectType = dependencyObjectType;
                DependencyPropertyName = dependencyPropertyName;
            }
        }

        private class DependencyPropertyInfoEqualityComparer : IEqualityComparer<DependencyPropertyInfo>
        {
            public bool Equals(DependencyPropertyInfo x, DependencyPropertyInfo y)
                => x.DependencyObjectType == y.DependencyObjectType && StringComparer.Ordinal.Equals(x.DependencyPropertyName, y.DependencyPropertyName);

            public int GetHashCode(DependencyPropertyInfo obj)
                => obj.DependencyObjectType.GetHashCode() ^ obj.DependencyPropertyName.GetHashCode();
        }
    }
}