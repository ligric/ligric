using CheburchayNavigation.Native.InfoModels;
using CheburchayNavigation.Native.Interfaces;
using CheburchayNavigation.Native.Notifications;
using CheburchayNavigation.Native.Notifications.EventArgs;
using System;

namespace CheburchayNavigation.Native
{
    public partial class NavigationService : IPagesService
    {
        public PageInfo CurrentPage { get; private set; }

        public event ElementsDirectionChangedHandler<PageInfo, PinInfo> CurrentPageChanged;

        public event PageExistenceChangedHandler PageExistenceChanged;

        public void SetCurrentPage(string pageKey)
        {
            var oldPageInfo = CurrentPage;

            if (string.IsNullOrEmpty(pageKey))
                throw new NullReferenceException($"Page key " + pageKey + " is null or empty.");

            if (!pages.TryGetValue(pageKey, out PageInfo outPageInfo))
                throw new NullReferenceException($"Page with key " + pageKey + " not found.");

            if (outPageInfo == null)
                throw new NullReferenceException("[404] page not found.");

            var newPageInfo = new PageInfo(outPageInfo.Element, outPageInfo.Key, Enums.SwitchState.Visible, outPageInfo.ViewModel, outPageInfo.BackPage, outPageInfo.NextPage);

            pages[pageKey] = newPageInfo;

            CurrentPage = newPageInfo;

            
            var pinChanges = UpdatePinsState(Enums.ElementDirection.Next);

            CurrentPageChanged?.Invoke(
                this, 
                new ElementDirectionChangedEventArgs<PageInfo>(Enums.ElementDirection.Next, newPageInfo, oldPageInfo),
                new ElementsDirectionChangedEventArgs<PinInfo>(Enums.ElementDirection.Next, pinChanges.NewPins, pinChanges.OldPins));
        }

        public void SetCurrentPage(object page, string pageKey, object vm = null, object backPage = null, object nextPage = null)
        {
            if (page == null)
                throw new NullReferenceException($"Prerender page is null.");

            if (string.IsNullOrEmpty(pageKey))
                throw new NullReferenceException($"Prerender page key " + pageKey + " is null or empty.");

            if (pages.TryGetValue(pageKey, out PageInfo outPage))
                throw new ArgumentException($"Page key " + pageKey + " already existence.");

            var newPageInfo = new PageInfo(page, pageKey, Enums.SwitchState.Visible, vm, backPage, nextPage);
            var oldPageInfo = CurrentPage;

            pages.Add(newPageInfo.Key, newPageInfo);

            PageExistenceChanged?.Invoke(this, PageExistenceAction.Prerender, newPageInfo);

            CurrentPage = newPageInfo;

            var pinChanges = UpdatePinsState(Enums.ElementDirection.Next);

            CurrentPageChanged?.Invoke(
                this,
                new ElementDirectionChangedEventArgs<PageInfo>(Enums.ElementDirection.Next, newPageInfo, oldPageInfo),
                new ElementsDirectionChangedEventArgs<PinInfo>(Enums.ElementDirection.Next, pinChanges.NewPins, pinChanges.OldPins));
        }

        public void PrerenderPage(object page, string pageKey, object vm = null, object backPage = null, object nextPage = null)
        {
            if (page == null)
                throw new NullReferenceException($"Prerender page is null.");

            if (string.IsNullOrEmpty(pageKey))
                throw new NullReferenceException($"Prerender page key " + pageKey + " is null or empty.");

            if (pages.TryGetValue(pageKey, out PageInfo outPage))
                throw new ArgumentException($"Page key " + pageKey + " already existence.");

            var newPageInfo = new PageInfo(page, pageKey, Enums.SwitchState.Collapsed, vm, backPage, nextPage);

            pages.Add(newPageInfo.Key, newPageInfo);
            PageExistenceChanged?.Invoke(this, PageExistenceAction.Prerender, newPageInfo);
        }
    }
}
