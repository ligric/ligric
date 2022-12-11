using Ligric.UI.Infrastructure;
using Prism.Ioc;
using Prism.Regions;

namespace Ligric.UI.ViewModels.Uno
{
    public class ShellViewModel
    {
        protected IRegionManager RegionManager { get; }

        public ShellViewModel(IContainerProvider containerProvider)
        {
            RegionManager = containerProvider.Resolve<IRegionManager>();

            RegionManager.RegisterViewWithRegion(NavigationRegions.CONTENT_REGION, NavigationViews.AUTHORIZATION);
        }
    }
}
