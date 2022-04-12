﻿using System;
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
        public object RootElement { get; }

        private enum PageActionEnum { Prerender, GoTo, GoWithPrerender }

        private Dictionary<string, PageInfo> activePages = new Dictionary<string, PageInfo>();
        private Dictionary<string, PinInfo> pins = new Dictionary<string, PinInfo>();

        public IReadOnlyDictionary<string, PageInfo> ActivePages { get; }

        public event CurrentPageEventHandler CurrentPageChanged;

        public event PagePrerenderEventHandler<PageInfo> ActivePagesChanged;

        public PageInfo CurrentPage { get; private set; }

        public NavigationService(object rootElement)
        {
            ActivePages = new ReadOnlyDictionary<string, PageInfo>(activePages);
            RootElement = rootElement;
        }

        public void PrerenderPage(object page, string pageName, object vm = null, string title = null, object backPage = null, object nextPage = null)
            => PageHandler(page, pageName, vm, backPage, nextPage, PageActionEnum.Prerender, title);

        public void GoTo(string pageName, object backPage = null, object nextPage = null) 
            => PageHandler(pageName: pageName, backPage : backPage, nextPage : nextPage, action : PageActionEnum.GoTo);

        public void GoWithPrerender(object page, string pageName = null, object vm = null, string title = null, object backPage = null, object nextPage = null)
            => PageHandler(page, pageName, vm, backPage, nextPage, PageActionEnum.GoWithPrerender, title);

        private int syncNumber = 0;
        private void PageHandler(object page = null, string pageName = null, object vm = null, object backPage = null, object nextPage = null, PageActionEnum action = 0, string title = null)
        {
            #region preparation
            var oldPage = CurrentPage;

            if (page is null && pageName is null)
            {
                throw new NullReferenceException("Page and page name are null.");
            }
                
            var prerenderPage = new PageInfo(page, pageName, vm, title, backPage, nextPage);
            #endregion

            switch (action)
            {
                case PageActionEnum.Prerender:
                    AddActivePage(prerenderPage, PageActiveAction.Prerender);
                    break;
                case PageActionEnum.GoWithPrerender:
                    AddActivePage(prerenderPage, PageActiveAction.GoWithPrerender);
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
                        CurrentPageChanged?.Invoke(this, RootElement, oldPage, outPage, PageChangingVectorEnum.Next, syncNumber++);
                        CurrentPage = outPage;
                    }
                    break;
            }
        }

        public void Pin(object pinElement, string pinKey, IEnumerable<string> forbiddenPageKeys, object viewModel, string rootKey = "root")
        {
            if(pinElement == null)
                throw new NullReferenceException("Pin element is null");
        }

        private void AddActivePage(PageInfo newPage, PageActiveAction action)
        {
            if (newPage is null)
                throw new NullReferenceException($"Page is null.");

            activePages.Add(newPage.PageKey, newPage);
            ActivePagesChanged?.Invoke(this, RootElement, action, newPage);
            newPage.PageClosed += OnPageClosed;
        }

        private void OnPageClosed(PageInfo page)
        {
            if (activePages.Remove(page.PageKey))
            {
                ActivePagesChanged?.Invoke(this, RootElement, PageActiveAction.Destroyed, page);
            }            
        }
    }
}
