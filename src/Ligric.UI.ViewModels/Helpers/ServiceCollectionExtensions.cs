using Ligric.Business.Apies;
using Ligric.Business.Authorization;
using Ligric.Business.Futures;
using Ligric.Business.Metadata;
using Ligric.Business.Subscriptions;

namespace Ligric.UI.ViewModels.Helpers;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddEndpoints(
		this IServiceCollection services,
		HostBuilderContext context,
		Action<IServiceProvider, RefitSettings>? settingsBuilder = null,
		bool useMocks = false)
	{
		_ = services
			// TEMP - this hsould be the default serialization options for content serialization > uno.extensions
			.AddSingleton(new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault })
			.AddNativeHandler()
			.AddContentSerializer();
		return services;
	}

	public static IServiceCollection AddServices(
		this IServiceCollection services,
		bool useMocks = false)
	{
		_ = services
			.AddSingleton<IMetadataManager, MetadataManager>()
			.AddSingleton<IAuthorizationService, AuthorizationService>()
			.AddSingleton<IApiesService, ApiesService>()
			.AddSingleton<IOrdersService, OrdersService>()
		    .AddSingleton<ISubscribeWebSockets, SubscribeWebSockets>()
			.AddSingleton<IMessenger, WeakReferenceMessenger>();

		if (useMocks)
		{
			// Comment out the USE_MOCKS definition (top of this file) to prevent using mocks in development
			//_ = services.AddSingleton<IAuthorizationService, MockAuthorizationService>();
		}
		return services;
	}
}
