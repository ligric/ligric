#pragma warning disable 109 // Remove warning for Window property on iOS

using Ligric.UI.Infrastructure.Presentation;
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

                // Add platform specific log providers
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

                        // Load OAuth configuration
                        .Section<Auth>()

                        // Load Mock configuration
                        .Section<Mock>()

                        // Enable app settings
                        //.Section<ToDoApp>()
                )

                // Register Json serializers (ISerializer and IStreamSerializer)
                .UseSerialization()

                // Register services for the application
                .ConfigureServices(
                    (context, services) => {

                        //var section = context.Configuration.GetSection(nameof(Mock));
                        //var useMocks = bool.TryParse(section[nameof(Mock.IsEnabled)], out var isMocked) ? isMocked : false;
#if USE_MOCKS
						// This is required for UI Testing where USE_MOCKS is enabled
						useMocks=true;;
#endif

                        //services
                        //    .AddScoped<IAppTheme, AppTheme>()
                        //    .AddEndpoints(context, useMocks: useMocks)
                        //    .AddServices(useMocks: useMocks);
                    })

                // Enable navigation, including registering views and viewmodels
                .UseNavigation(Infrastructure.ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)

                // Add navigation support for toolkit controls such as TabBar and NavigationView
                .UseToolkitNavigation()

                // Add localization support
                .UseLocalization()

                .Build(enableUnoLogging: true);
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap<AuthorizationPage, AuthorizationViewModel>(),
            new ViewMap(ViewModel: typeof(ShellViewModel))
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellViewModel>(), Nested: new RouteMap[]
            {
                new("Authorization", View: views.FindByViewModel<AuthorizationViewModel>()),
            })
        );
    }
}
