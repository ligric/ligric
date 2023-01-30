using CheburchayNavigation.Native.Enums;
using System;
using System.Collections.Generic;

namespace CheburchayNavigation.Native.InfoModels
{
    public delegate void PageClosedHandler(PageInfo page);

    public sealed class PageInfo : IEquatable<PageInfo>, IModelInfo
    {
        public object Element { get; }

        public object ViewModel { get; }

        public string Key { get; }

        public object BackPage { get; }

        public object NextPage { get; }

        public SwitchState State { get; }

        private readonly int hash;

        public PageInfo(object page, string pageKey, SwitchState state = SwitchState.Collapsed, object vm = null, object backPage = null, object nextPage = null)
        {
            Element = page == null ? throw new NullReferenceException("Page is null.") : page;
            Key = string.IsNullOrEmpty(pageKey) ? throw new NullReferenceException("Page key is null or empty.") : pageKey;
            State = state;
            ViewModel = vm;
            BackPage = backPage; 
            NextPage = nextPage;

            hash = CalculateHash();
        }

        #region Equals
        public bool Equals(PageInfo other)
        {
            return Equals(other.Element, Element) &&
                   Equals(other.Key, Key) &&
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
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Element);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(BackPage);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(NextPage);
            return hashCode;
        }
        #endregion
    }
}
