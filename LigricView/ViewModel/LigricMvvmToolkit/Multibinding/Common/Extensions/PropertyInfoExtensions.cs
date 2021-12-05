using System.Reflection;

namespace LigricMvvmToolkit.Multibinding.Common.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool CanRead(this PropertyInfo propertyInfo)
            => propertyInfo.CanRead && !propertyInfo.GetMethod.IsFamily && !propertyInfo.GetMethod.IsPrivate;

        public static bool CanWrite(this PropertyInfo propertyInfo)
            => propertyInfo.CanWrite && !propertyInfo.SetMethod.IsFamily && !propertyInfo.SetMethod.IsPrivate;
    }
}