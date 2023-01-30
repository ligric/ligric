using CheburchayNavigation.Native.InfoModels;
using CheburchayNavigation.Native.Notifications;
using System.Collections.Generic;

namespace CheburchayNavigation.Native.Interfaces
{
    public enum PageExistenceAction
    {
        Prerender,
        Destroy
    }

    public delegate void PageExistenceChangedHandler(object sender, PageExistenceAction action, PageInfo pageInfo);

    public interface IPagesService
    {
        PageInfo CurrentPage { get; }

        IReadOnlyDictionary<string, PageInfo> Pages { get; }

        event ElementsDirectionChangedHandler<PageInfo, PinInfo> CurrentPageChanged;

        event PageExistenceChangedHandler PageExistenceChanged;

        void SetCurrentPage(string pageKey);

        void SetCurrentPage(object page, string pageKey, object vm = null, object backPage = null, object nextPage = null);

        void PrerenderPage(object page, string pageKey, object vm = null, object backPage = null, object nextPage = null);
    }
}
