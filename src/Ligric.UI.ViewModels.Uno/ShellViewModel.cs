using ReactiveUI;
using System;

namespace Ligric.UI.ViewModels.Uno
{
    public class ShellViewModel : ReactiveObject, IScreen, IActivatableViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        public ShellViewModel()
        {
            //_serviceProvider = serviceProvider;
        }
        public RoutingState Router { get; } = new RoutingState();

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
