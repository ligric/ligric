using CheburchayNavigation.Native;
using CheburchayNavigation.Native.Interfaces;
using System;

namespace Uno.CheburchayNavigation.InfoModels
{
    public sealed class WrapperInfo
    {
        public string Key { get; }

        public object Wrapper { get; }

        public INavigationService Navigation { get; }

        public WrapperInfo(string key, object wrapper, INavigationService navigation)
        {
            Key = key ?? throw new NullReferenceException("Wrapper key is null.");
            Wrapper = wrapper ?? throw new NullReferenceException("Wrapper is null.");
            Navigation = navigation ?? throw new NullReferenceException("Navigation is null.");
        }
    }
}
