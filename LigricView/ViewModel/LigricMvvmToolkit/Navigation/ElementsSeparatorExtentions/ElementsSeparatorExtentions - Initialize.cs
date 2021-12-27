using System.Collections.Generic;
using System;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    internal static partial class ElementsSeparatorExtentions
    {
        private class WrapperInfo
        {
            public string Key { get; }

            public Panel Wrapper { get; }

            public int PinsCount { get; }

            public WrapperInfo(string key, Panel wrapper, int pinsCount)
            {
                Key = key ?? throw new ArgumentNullException("Key is null.");  
                Wrapper = wrapper ?? throw new ArgumentNullException("Wrapper is null.");
                PinsCount = pinsCount >= 0 ? pinsCount : throw new ArgumentException("Pins count cannot be less than zero.");
            }
        }

        private readonly static Dictionary<string, WrapperInfo> wrappers = new Dictionary<string, WrapperInfo>();
    }
}
