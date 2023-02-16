using System.Data;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Ligric.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection Remove<T>(this IServiceCollection services)
		{
			if (services.IsReadOnly)
			{
				throw new ReadOnlyException($"{nameof(services)} is read only");
			}

			var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
			if (serviceDescriptor != null) services.Remove(serviceDescriptor);

			return services;
		}
	}
}
