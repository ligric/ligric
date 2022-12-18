using ReactiveUI;
using Splat;

namespace Ligric.UI.ViewModels.Uno
{
    public abstract class RoutableViewModel : ReactiveObject, IRoutableViewModel
    {
        protected RoutableViewModel(IScreen? screen = null)
        {
            HostScreen = screen;

        }

        public string UrlPathSegment { get; }
        public IScreen HostScreen { get; }
    }
}
