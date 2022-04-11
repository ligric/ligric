using LigricMvvmToolkit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LigricMvvmToolkit.Navigation
{
    internal static partial class ElementsSeparatorExtensions
    {
        public static Panel AddWrapper(this FrameworkElement element, string rootKey = "root")
        {
            SolidColorBrush GetSolidColorBrush(string hex)
            {
                hex = hex.Replace("#", string.Empty);
                byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
                byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
                byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
                byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
                SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
                return myBrush;
            }


            var parent = element.Parent;
            if (parent is null)
                throw new TypeAccessException("Root parent element is null.");

            if (string.IsNullOrEmpty(rootKey))
                throw new ArgumentNullException("Root key cannot be null or empty.");

            if (wrappers.TryGetValue(rootKey, out WrapperInfo wrapperInfo))
                return wrapperInfo.Wrapper;

            Panel wrapper = new Grid() { Tag = rootKey + "_wrapper", Background = GetSolidColorBrush("#FFcbcbcd") };
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
            parent.Children.Remove(element);

            wrapper.Children.Add(element);

            parent.Children.Add(wrapper);

            return wrapper;
        }

        private static Panel AddWrapperInTheBorder(this FrameworkElement element, Border parent, Panel wrapper)
        {
            parent.Child = null;

            wrapper.Children.Add(new Border() { Child = element, Tag = "Element wrapper" } );

            parent.Child = wrapper;

            return wrapper;
        }
        #endregion

        private static int testCount = 0;

        public static FrameworkElement AddElementToWrapper(this Panel wrapper, FrameworkElement addElement)
        {

            var wrapperInfo = GetWrapperInfo(wrapper);

            var count = wrapper.Children.Count;

            var elementWrapper = new Border() { Visibility = Visibility.Visible, Child = addElement, Tag = "Element wrapper " + testCount++ };

            // TODO : There can be to math problems.
            // should to check with add
            SetElementHidenPosition(wrapper, elementWrapper);

            wrapper.Children.Insert(0, elementWrapper);

            return addElement;
        }

        private static void SetElementHidenPosition(this Panel wrapper, FrameworkElement elementWrapper)
        {
            EventHandler<object> wrapperLayoutUpdatedstroyboard = null;

            wrapperLayoutUpdatedstroyboard = (s, e) =>
            {
                wrapper.LayoutUpdated -= wrapperLayoutUpdatedstroyboard;

                EventHandler<object> elementWrapperLayoutUpdatedstroyboard = null;

                elementWrapperLayoutUpdatedstroyboard = (ss, ee) =>
                {
                    elementWrapper.LayoutUpdated -= elementWrapperLayoutUpdatedstroyboard;

                    elementWrapper.TransformInitialize();
                    var fullWidth = elementWrapper.GetFulllWidth(wrapper);
                    var tag = elementWrapper.Tag;
                    ((TranslateTransform)elementWrapper.RenderTransform).X = wrapper.ActualWidth > fullWidth ? -wrapper.ActualWidth : -fullWidth;
                };

                elementWrapper.LayoutUpdated += elementWrapperLayoutUpdatedstroyboard;
            };
            wrapper.LayoutUpdated += wrapperLayoutUpdatedstroyboard;
        }

        public static Panel AddPinElement(this Panel wrapper, FrameworkElement element, string currentPageKey = null, IReadOnlyCollection<string> forbiddenPageKeys = null)
        {
            var wrapperInfo = GetWrapperInfo(wrapper);

            if (!string.IsNullOrEmpty(currentPageKey) && forbiddenPageKeys != null)
            {
                if (forbiddenPageKeys.FirstOrDefault(x => x.Contains(currentPageKey)) != null)
                {
                    SetElementHidenPosition(wrapper, element);
                }
            }

            wrapperInfo.Wrapper.Children.Add(element);
            wrapperInfo.Pins.Add(new PinInfo(wrapper, element, forbiddenPageKeys));
            wrappers[wrapperInfo.Key] = new WrapperInfo(wrapperInfo.Key, wrapperInfo.Wrapper, wrapperInfo.Pins);

            return wrapper;
        }

        public static (IReadOnlyCollection<FrameworkElement> BlockedPins, IReadOnlyCollection<FrameworkElement> AvailablePins) GetPins(string rootKey, string pageKey)
        {
            if (!wrappers.TryGetValue(rootKey, out WrapperInfo wrapperInfo))
                throw new ArgumentException($"[404] Element with \"{rootKey}\" key not found");

            List<FrameworkElement> blockedPins = new List<FrameworkElement>();
            List<FrameworkElement> availablePins = new List<FrameworkElement>();
            foreach (var pinInfo in wrapperInfo.Pins)
            {
                if (pinInfo.ForbiddenPageKeys.Contains(pageKey))
                {
                    blockedPins.Add(pinInfo.Pin);
                }
                else
                {
                    availablePins.Add(pinInfo.Pin);
                }
            }

            return (blockedPins, availablePins);
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
