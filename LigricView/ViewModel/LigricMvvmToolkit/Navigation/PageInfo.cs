using System;
using System.Collections.Generic;

namespace LigricMvvmToolkit.Navigation
{
    public delegate void PageClosedHandler(PageInfo page);

    public class PageInfo : IDisposable, IEquatable<PageInfo>
    {
        public object Page { get; }

        public object ViewModel { get; }

        /// <summary> 
        /// Unique name for easy search. 
        /// </summary>
        /// <remarks> 
        /// Default value -- <c>nameof(Page)</c> 
        /// </remarks>
        public string PageKey { get; }

        /// <summary> 
        /// The name of the page to be displayed on the view. 
        /// </summary>
        /// <remarks> 
        /// Default value -- <c>nameof(PageName)</c> 
        /// </remarks>
        public string Title { get; }

        /// <summary> 
        /// Reference to the page that came before this page. 
        /// </summary>
        public object BackPage { get; }

        public object NextPage { get; }

        public event PageClosedHandler PageClosed;

        private readonly int hash;

        public PageInfo(object page, string pageKey, object vm = null, string title = null, object backPage = null, object nextPage = null)
        {
            Page = page;
            PageKey = string.IsNullOrEmpty(pageKey) ? nameof(page) : pageKey;
            ViewModel = vm;
            Title = string.IsNullOrEmpty(title) ? PageKey : title; 
            BackPage = backPage; 
            NextPage = nextPage;

            hash = CalculateHash();
        }

        #region Equals
        public bool Equals(PageInfo other)
        {
            return Equals(other.Page, Page) &&
                   Equals(other.PageKey, PageKey) &&
                   Equals(other.Title, Title) &&
                   Equals(other.BackPage, BackPage) &&
                   Equals(other.NextPage, NextPage);
        }

        public override bool Equals(object obj)
        {
            return obj is PageInfo pageInfo && Equals(pageInfo);
        }
        #endregion

        #region GetHashCode
        public override int GetHashCode() => hash;
        private int CalculateHash()
        {
            int hashCode = 1365913784;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Page);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PageKey);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(BackPage);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(NextPage);
            return hashCode;
        }
        #endregion

        #region Dispose
        private bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                PageClosed?.Invoke(this);
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
