using CheburchayNavigation.Native.InfoModels;
using CheburchayNavigation.Native.Interfaces;
using System.Collections.ObjectModel;

namespace CheburchayNavigation.Native
{
    public partial class NavigationService : INavigationService
    {
        public string RootKey { get; }

        public NavigationService(string rootKey)
        {
            RootKey = rootKey;
            Pages = new ReadOnlyDictionary<string, PageInfo>(pages);
            Pins = new ReadOnlyDictionary<string, PinInfo>(pins);
        }
    }
}
