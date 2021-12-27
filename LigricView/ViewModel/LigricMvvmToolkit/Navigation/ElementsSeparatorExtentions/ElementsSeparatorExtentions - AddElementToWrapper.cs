﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    internal static partial class ElementsSeparatorExtentions
    {
        public static FrameworkElement AddElementToWrapper(this Panel wrapper, FrameworkElement addElement)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException("Wrapper element is null");
            }

            wrapper.Children.Add(addElement);

            return addElement;
        }

        public static Panel AddPinElement(this Panel wrapper, FrameworkElement element, IReadOnlyCollection<string> forbiddenPageKeys = null)
        {
            var wrapperInfo = wrappers.Values.FirstOrDefault(x => x.Wrapper == wrapper);
            if (wrapperInfo == null)
                throw new ArgumentException("Unknown object. Please user the RegisterRoot mathod.");

            wrapperInfo.Wrapper.Children.Add(element);
            wrapperInfo.Pins.Add(new PinInfo(wrapper, element, forbiddenPageKeys));
            wrappers[wrapperInfo.Key] = new WrapperInfo(wrapperInfo.Key, wrapperInfo.Wrapper, wrapperInfo.Pins);

            return wrapper;
        }
    }
}
