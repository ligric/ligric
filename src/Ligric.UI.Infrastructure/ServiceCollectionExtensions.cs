namespace Ligric.UI.Infrastructure;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddEndpoints(
		this IServiceCollection services,
		HostBuilderContext context,
		Action<IServiceProvider, RefitSettings>? settingsBuilder = null,
		bool useMocks=false)
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
		   .AddSingleton<IAuthenticationService, AuthenticationService>()
		   .AddSingleton<IAuthenticationTokenProvider>(sp => sp.GetRequiredService<IAuthenticationService>())
		   .AddSingleton<IMessenger, WeakReferenceMessenger>();

		if (useMocks)
		{
			// Comment out the USE_MOCKS definition (top of this file) to prevent using mocks in development
			_ = services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
		}
		return services;
	}
}
