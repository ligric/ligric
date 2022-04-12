using System;

namespace LigricMvvmToolkit.Navigation
{
    public class WrapperInfo
    {
        public string Key { get; }

        public object Wrapper { get; }

        public WrapperInfo(string key, object wrapper)
        {
            Key = key ?? throw new ArgumentNullException("Wrapper key is null.");
            Wrapper = wrapper ?? throw new ArgumentNullException("Wrapper is null.");
        }
    }
}
