using System;

namespace LigricUno.LocalViewServices
{
    internal static class NavigationItemsService
    {
        public static event Action<string> ItemSelected;

        public static void RaiseItemSelectedAction(string itemKey)
            => ItemSelected?.Invoke(itemKey);
    }
}
