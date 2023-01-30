using CheburchayNavigation.Native.InfoModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CheburchayNavigation.Native
{
    public partial class NavigationService
    {
        private readonly Dictionary<string, PageInfo> pages = new Dictionary<string, PageInfo>();
        private readonly Dictionary<string, PinInfo> pins = new Dictionary<string, PinInfo>();

        public IReadOnlyDictionary<string, PageInfo> Pages { get; }
        public IReadOnlyDictionary<string, PinInfo> Pins { get; }
    }
}
