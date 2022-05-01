using System;

namespace LigricUno.LocalViewServices
{
    internal static class NavigationItemsService
    {
        public static event Action<string> HeaderOptionItemSelected;

        public static void RaiseItemSelectedAction(string itemKey)
            => HeaderOptionItemSelected?.Invoke(itemKey);
    }
}
