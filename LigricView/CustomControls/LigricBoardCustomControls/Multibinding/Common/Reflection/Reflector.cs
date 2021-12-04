using System;
using System.Reflection;

namespace WinRTMultibinding.Common.Reflection
{
    internal static class Reflector
    {
        public static TMemberInfo ScanHierarchyForMember<TMemberInfo>(Type type, Func<TypeInfo, TMemberInfo> memberExtractor) where TMemberInfo : MemberInfo
        {
            while (type != typeof(object))
            {
                var typeInfo = type.GetTypeInfo();
                TMemberInfo memberInfo;
                if ((memberInfo = memberExtractor(typeInfo)) != null)
                {
                    return memberInfo;
                }

                type = typeInfo.BaseType;
            }

            return null;
        }
    }
}