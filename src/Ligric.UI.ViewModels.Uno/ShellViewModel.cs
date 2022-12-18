using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;

namespace Ligric.UI.ViewModels.Uno
{
    public class ShellViewModel : IScreen, IActivatableViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        public ShellViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            //var test = Router.NavigationStack;
            //var res = _serviceProvider.GetRequiredService<AuthorizationViewModel>();
            //Router.Navigate.InvokeCommand((System.Windows.Input.ICommand?)res);
        }
        public RoutingState Router { get; } = new RoutingState();

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
