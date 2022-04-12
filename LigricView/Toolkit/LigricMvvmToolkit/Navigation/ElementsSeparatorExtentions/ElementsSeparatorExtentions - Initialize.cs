using System.Collections.Generic;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace LigricMvvmToolkit.Navigation
{
    internal static partial class ElementsSeparatorExtensions
    {
        private class WrapperInfo
        {
            public string Key { get; }

            public Panel Wrapper { get; }

            public IList<PinInfo> Pins { get; }

            public WrapperInfo(string key, Panel wrapper, IList<PinInfo> pins)
            {
                Key = key ?? throw new ArgumentNullException("Key is null.");  
                Wrapper = wrapper ?? throw new ArgumentNullException("Wrapper is null.");
                Pins = pins ?? new List<PinInfo>();
            }
        }

        private readonly static Dictionary<string, WrapperInfo> wrappers = new Dictionary<string, WrapperInfo>();
    }
}
