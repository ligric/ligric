using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Messaging;
using Ligric.Business.Apies;
using Ligric.Business.Authorization;
using Ligric.Business.Clients;
using Ligric.Business.Clients.Authorization;
using Ligric.Business.Futures;
using Ligric.Business.Interfaces;
using Ligric.Business.Metadata;
using Refit;

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
		var grpcChannel = GrpcChannelHalper.GetGrpcChannel();
		var metadata = new MetadataManager();
		var authorization = new AuthorizationService(grpcChannel, metadata);
		var cryptoClient = new LigricCryptoClient(grpcChannel, authorization, metadata);

		_ = services
			.AddSingleton(grpcChannel)

			.AddSingleton<IMetadataManager>(metadata)
			.AddSingleton<IAuthorizationService>(authorization)
			.AddSingleton<ILigricCryptoClient>(cryptoClient)

			.AddSingleton(cryptoClient.Apis)
			.AddSingleton(cryptoClient.Orders)
			.AddSingleton(cryptoClient.Values)
			.AddSingleton(cryptoClient.Positions)

			.AddSingleton<IMessenger, WeakReferenceMessenger>();

		if (useMocks)
		{
			// Comment out the USE_MOCKS definition (top of this file) to prevent using mocks in development
			//_ = services.AddSingleton<IAuthorizationService, MockAuthorizationService>();
		}
		return services;
	}
}
