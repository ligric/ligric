#pragma warning disable 109 // Remove warning for Window property on iOS

using Ligric.UI.ViewModels.Helpers;
using Ligric.UI.Views;

namespace Ligric.UI;

public sealed partial class App : Application
{
    private IHost? _host { get; set; }

    private static IHost BuildAppHost()
    {
        return UnoHost
                .CreateDefaultBuilder()
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif

                // AddAndRiseEvent platform specific log providers
                .UseLogging(configure: (context, logBuilder) =>
                            // Configure log levels for different categories of logging
                            logBuilder
                                    .SetMinimumLevel(
                                        context.HostingEnvironment.IsDevelopment() ?
                                            LogLevel.Information :
                                            LogLevel.Warning))

                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        // Load configuration information from appconfig.json
                        .EmbeddedSource<App>()
                        .EmbeddedSource<App>("platform")
                )

                // Register Json serializers (ISerializer and IStreamSerializer)
                .UseSerialization()

                // Register services for the application
                .ConfigureServices(
                    (context, services) => {
						services.AddServices(useMocks:false);
						//var section = context.Configuration.GetSection(nameof(Mock));
						//var useMocks = bool.TryParse(section[nameof(Mock.IsEnabled)], out var isMocked) ? isMocked : false;
#if USE_MOCKS
						// This is required for UI Testing where USE_MOCKS is enabled
						useMocks=true;
#endif
					})

                // Enable navigation, including registering views and viewmodels
                //.UseNavigation(ViewModels.ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)

                // AddAndRiseEvent navigation support for toolkit controls such as TabBar and NavigationView
                .UseToolkitNavigation()

                // AddAndRiseEvent localization support
                .UseLocalization()

                .Build(enableUnoLogging: true);
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
   //     views.Register(
   //         new ViewMap<AuthorizationPage, AuthorizationViewModel>(),
   //         new ViewMap<FuturesPage, FuturesViewModel>(),
   //         new ViewMap(ViewModel: typeof(ShellViewModel))
   //     );

   //     routes.Register(
   //         new RouteMap("", View: views.FindByViewModel<ShellViewModel>(), Nested: new RouteMap[]
   //         {
   //             new("Authorization", View: views.FindByViewModel<AuthorizationViewModel>()),
   //             new("Futures", View: views.FindByViewModel<FuturesViewModel>())
			//})
   //     );
    }
}
