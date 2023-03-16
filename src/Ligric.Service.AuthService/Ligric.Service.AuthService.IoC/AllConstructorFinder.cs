using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace Ligric.Service.AuthService.IoC
{
    internal class AllConstructorFinder : IConstructorFinder
    {
        private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache =
            new ConcurrentDictionary<Type, ConstructorInfo[]>();


        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            var result = Cache.GetOrAdd(targetType,
                t => t.GetTypeInfo().DeclaredConstructors.ToArray());

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			return result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		}
    }
}
