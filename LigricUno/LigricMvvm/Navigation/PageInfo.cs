﻿using System;

namespace LigricMvvm.Navigation
{
    internal delegate void PageClosedHandler(PageInfo sender);

    internal class PageInfo
    {
        public object BackPage { get; }

        public object Page { get; }

        public string PageName { get; }

        public object NextPage { get; }

        public event PageClosedHandler PageClosed;


        public PageInfo(object page, string pageName, object backPage = null, object nextPage = null)
        {
            Page = page; PageName = pageName; BackPage = backPage; NextPage = nextPage;
        }
    }
}
