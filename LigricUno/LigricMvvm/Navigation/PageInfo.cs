using System;

namespace LigricMvvm.Navigation
{
    internal delegate void PageClosedHandler(string pageName);

    internal class PageInfo : IDisposable
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

        #region Dispose
        private bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                PageClosed?.Invoke(PageName);
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~PageInfo()
        {
            Dispose();
        }
        #endregion
    }
}
