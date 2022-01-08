﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    internal static partial class ElementsSeparatorExtensions
    {
        public static Panel AddWrapper(this FrameworkElement element, string rootKey = "root")
        {
            var parent = element.Parent;
            if (parent is null)
                throw new TypeAccessException("Root parent element is null.");

            if (string.IsNullOrEmpty(rootKey))
                throw new ArgumentNullException("Root key cannot be null or empty.");

            if (wrappers.TryGetValue(rootKey, out WrapperInfo wrapperInfo))
                return wrapperInfo.Wrapper;


            Panel wrapper = new Grid() { Tag = rootKey + "_wrapper" };
            if (parent is UserControl)
            {
                wrapper = element.AddWrapperInTheUserControl((UserControl)parent, wrapper);
            }
            else if (parent is Panel)
            {
                wrapper = element.AddWrapperInThePanel((Panel)parent, wrapper);
            }
            else if (parent is Border)
            {
                wrapper = element.AddWrapperInTheBorder((Border)parent, wrapper);
            }
            else
            {
                throw new NotImplementedException($"Type {parent} not implemented");
            }

            wrappers.Add(rootKey, new WrapperInfo(rootKey, wrapper, null));

            return wrapper;
        }

        #region AddWrapper different types handler
        private static Panel AddWrapperInTheUserControl(this FrameworkElement element, UserControl parent, Panel wrapper)
        {
            parent.Content = null;

            wrapper.Children.Add(element);

            parent.Content = wrapper;

            return wrapper;
        }

        private static Panel AddWrapperInThePanel(this FrameworkElement element, Panel parent, Panel wrapper)
        {
            foreach (var item in parent.Children)
            {
                if (item == element)
                {
                    parent.Children.Remove(item);
                }
            }

            wrapper.Children.Add(element);

            parent.Children.Add(wrapper);

            return wrapper;
        }

        private static Panel AddWrapperInTheBorder(this FrameworkElement element, Border parent, Panel wrapper)
        {
            parent.Child = null;

            wrapper.Children.Add(element);

            parent.Child = wrapper;

            return wrapper;
        }
        #endregion

        public static FrameworkElement AddElementToWrapper(this Panel wrapper, FrameworkElement addElement)
        {
            var wrapperInfo = GetWrapperInfo(wrapper);

            var count = wrapper.Children.Count;

            wrapper.Children.Insert(count - wrapperInfo.Pins.Count, addElement);

            return addElement;
        }

        public static Panel AddPinElement(this Panel wrapper, FrameworkElement element, IReadOnlyCollection<string> forbiddenPageKeys = null)
        {
            var wrapperInfo = GetWrapperInfo(wrapper);

            wrapperInfo.Wrapper.Children.Add(element);
            wrapperInfo.Pins.Add(new PinInfo(wrapper, element, forbiddenPageKeys));
            wrappers[wrapperInfo.Key] = new WrapperInfo(wrapperInfo.Key, wrapperInfo.Wrapper, wrapperInfo.Pins);

            return wrapper;
        }

        public static IReadOnlyCollection<FrameworkElement> GetBlockedPins(string rootKey, string blockedKey)
        {
            if (!wrappers.TryGetValue(rootKey, out WrapperInfo wrapperInfo))
                throw new ArgumentException($"[404] Element with \"{rootKey}\" key not found");

            List<FrameworkElement> outputPins = new List<FrameworkElement>();
            foreach (var pinInfo in wrapperInfo.Pins)
            {
                if (pinInfo.ForbiddenPageKeys.Contains(blockedKey))
                {
                    outputPins.Add(pinInfo.Pin);
                }
            }

            return outputPins;
        }

        private static WrapperInfo GetWrapperInfo(Panel wrapper)
        {
            if (wrapper == null)
                throw new ArgumentNullException("Wrapper element is null");

            var wrapperInfo = wrappers.Values.FirstOrDefault(x => x.Wrapper == wrapper);
            if (wrapperInfo == null)
                throw new ArgumentException("Unknown object. Please user the RegisterRoot mathod.");

            return wrapperInfo;
        }
    }
}