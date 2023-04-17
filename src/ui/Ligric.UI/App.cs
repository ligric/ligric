#pragma warning disable 109 // Remove warning for Window property on iOS

namespace Ligric.UI
{
    public partial class App : Application
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        private Window? _window;
        public new Window? Window => _window;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {

#if NET7_0_OR_GREATER && WINDOWS && !HAS_UNO
            _window = new Window();
#else
            _window = Microsoft.UI.Xaml.Window.Current;
#endif
			var appRoot = new Shell();
			appRoot.SplashScreen.Initialize(_window, args);

			_window.Content = appRoot;
			_window.Activate();

			_host = await _window.InitializeNavigationAsync(
					async () =>
					{
						return BuildAppHost();
					},
					navigationRoot: appRoot.SplashScreen
				);

			var notif = _host.Services.GetRequiredService<IRouteNotifier>();
			notif.RouteChanged += RouteUpdated;
		}

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public void RouteUpdated(object? sender, RouteChangedEventArgs e)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            try
            {
                var rootRegion = e.Region.Root();
                var route = rootRegion.GetRoute();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
