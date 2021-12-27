using System.Collections.Generic;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace LigricMvvmToolkit.Navigation
{
    internal static partial class ElementsSeparatorExtentions
    {
        private class PinInfo
        {
            public Panel Wrapper { get; }

            public FrameworkElement Pin { get; }

            public IReadOnlyCollection<string> ForbiddenPageKeys { get; }

            public PinInfo(Panel wrapper, FrameworkElement pin, IReadOnlyCollection<string> forbiddenPageKeys)
            {
                Wrapper = wrapper ?? throw new ArgumentNullException("Wrapper cannot be null.");
                Pin = pin ?? throw new ArgumentNullException("Pin cannot be null.");
                ForbiddenPageKeys = forbiddenPageKeys ?? new List<string>();
            }
        }

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
