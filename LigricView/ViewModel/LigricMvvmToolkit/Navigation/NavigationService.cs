using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LigricMvvmToolkit.Navigation
{
    

    //public delegate void ActivePageAddedHandler(object sender, object page, PageActionEnum action);


    public class NavigationService : INavigationService
    {
        private enum PageActionEnum { Prerender, GoTo }

        private Dictionary<string, PageInfo> activePages = new Dictionary<string, PageInfo>();

        public event CurrentPageChangeHandler CurrentPageChanged;

        public object CurrentPage { get; private set; }

        public Task PrerenderPage(object page, string pageName = null, object backPage = null, object nextPage = null)
            => PageHandler(page, pageName, backPage, nextPage, PageActionEnum.Prerender);

        public Task GoTo(string pageName, object page = null, object backPage = null, object nextPage = null) 
            => PageHandler(page, pageName, backPage, nextPage, PageActionEnum.GoTo);


        private Task PageHandler(object page, string pageName = null, object backPage = null, object nextPage = null, PageActionEnum action = 0) => Task.Run(() =>
        {
            var oldPage = CurrentPage;

            if (page is null && pageName is null)
                throw new ArgumentException("Page name and page are null.");

            string resultPageName = pageName;
            if (string.IsNullOrEmpty(resultPageName))
                resultPageName = nameof(page);

            switch (action)
            {
                case PageActionEnum.Prerender:
                    if (page is null)
                        throw new ArgumentException($"Page is null.");

                    break;
                case PageActionEnum.GoTo:
                    if (activePages.TryGetValue(resultPageName, out PageInfo outPage))
                    {
                        CurrentPageChanged?.Invoke(this, oldPage, outPage.Page, PageChangingVectorEnum.Next);
                    }  
                    else if(page is null)
                    {
                        throw new ArgumentException("[404] page not found.");
                    }
                    else
                    {
                        if (AddActivePage(new PageInfo(page, resultPageName, backPage, nextPage)))
                        {
                            CurrentPageChanged?.Invoke(this, oldPage, page, PageChangingVectorEnum.Next);
                        }
                    }
                    break;
            }
        });

        private bool AddActivePage(PageInfo newPage)
        {
 
            //if (!activePages.TryAdd(newPage.PageName, newPage))
                //return false;

            try
            {
                activePages.Add(newPage.PageName, newPage);
            }
            catch
            {
                return false;
            }

            newPage.PageClosed += OnPageClosed;

            return true;
        }

        private void OnPageClosed(string pageName)
        {
            activePages.Remove(pageName);
        }
    }
}
