using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LigricMvvm.Navigation
{
    public sealed class NavigationService : INavigationService
    {
        private Dictionary<string, PageInfo> pages = new Dictionary<string, PageInfo>();

        public event PageChangeHandler PageChanged;

        public object CurrentPage { get; private set; }

        public Task PrerenderPage(object page, string pageName = null, object backPage = null, object nextPage = null) => Task.Run(() => 
        {
            #region Protections
            if (pages.TryGetValue(pageName, out PageInfo outPage))
                throw new ArgumentException($"Page with name \"{pageName}\" already registred.");

            if (page == null)
                throw new ArgumentException($"Page is null.");
            #endregion

            var newPage = new PageInfo(page, pageName, backPage, nextPage);
            newPage.PageClosed += OnPageClosed;
            pages.Add(pageName, newPage);
        });

        public Task GoTo(string pageName) => Task.Run(() => 
        {
            if (pages.TryGetValue(pageName, out PageInfo outPage))
            {
                PageChanged?.Invoke(this, CurrentPage, outPage.Page, PageChangingVectorEnum.Next);
            }
        });

        private void OnPageClosed(PageInfo sender)
        {
            throw new NotImplementedException();
        }
    }
}
