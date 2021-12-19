using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using Common.Delegates;
using Common.Enums;

namespace LigricMvvmToolkit.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly object rootElement;
        private enum PageActionEnum { Prerender, GoTo }

        private Dictionary<string, PageInfo> activePages = new Dictionary<string, PageInfo>();
        public IReadOnlyDictionary<string, PageInfo> ActivePages { get; }

        public event CurrentPageEventHandler CurrentPageChanged;

        public event CollectionEventHandler<PageInfo> ActivePagesChanged;

        public PageInfo CurrentPage { get; private set; }

        public NavigationService(object rootElement)
        {
            ActivePages = new ReadOnlyDictionary<string, PageInfo>(activePages);
            this.rootElement = rootElement;
        }

        public void PrerenderPage(object page, string pageName = null, string title = null, object backPage = null, object nextPage = null)
            => PageHandler(page, pageName, backPage, nextPage, PageActionEnum.Prerender, title);

        public void GoTo(string pageName, object backPage = null, object nextPage = null) 
            => PageHandler(pageName: pageName, backPage : backPage, nextPage : nextPage, action : PageActionEnum.GoTo);

        private void PageHandler(object page = null, string pageName = null, object backPage = null, object nextPage = null, PageActionEnum action = 0, string title = null)
        {
            #region preparation
            var oldPage = CurrentPage;

            if (page is null && pageName is null)
            {
                throw new NullReferenceException("Page and page name are null.");
            }
                
            var prerenderPage = new PageInfo(page, pageName, title, backPage, nextPage);
            #endregion

            switch (action)
            {
                case PageActionEnum.Prerender:

                    if (page is null)
                    {
                        throw new NullReferenceException($"Page is null.");
                    }

                    AddActivePage(prerenderPage);

                    break;
                case PageActionEnum.GoTo:

                    if (!activePages.TryGetValue(prerenderPage.PageKey, out PageInfo outPage))
                    {
                        throw new NullReferenceException($"Page is null.");
                    }

                    if (outPage is null)
                    {
                        throw new NullReferenceException("[404] page not found.");
                    }
                    else
                    {
                        CurrentPageChanged?.Invoke(this, rootElement, oldPage, outPage, PageChangingVectorEnum.Next);
                    }
                    break;
            }
        }

        private bool AddActivePage(PageInfo newPage)
        {
            try
            {
                activePages.Add(newPage.PageKey, newPage);
                ActivePagesChanged?.Invoke(this, rootElement, ActionCollectionEnum.Added, newPage);
                newPage.PageClosed += OnPageClosed;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void OnPageClosed(PageInfo page)
        {
            if (activePages.Remove(page.PageKey))
            {
                ActivePagesChanged?.Invoke(this, rootElement, ActionCollectionEnum.Removed, page);
            }            
        }
    }
}
