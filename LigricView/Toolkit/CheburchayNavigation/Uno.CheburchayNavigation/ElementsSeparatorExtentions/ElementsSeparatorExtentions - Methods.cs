using System;
using System.Collections.Generic;
using System.Linq;
using Uno.CheburchayNavigation.Extensions;
using Uno.CheburchayNavigation.InfoModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LigricMvvmToolkit.Navigation
{
    internal static partial class ElementsSeparatorExtensions
    {
        public static Panel AddWrapper(this FrameworkElement element, Action addFinished)
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

            Panel wrapper = new Grid() { Background = GetSolidColorBrush("#FFcbcbcd") };

            wrapper.ElementAddFinished(addFinished);

            if (parent is UserControl parentUserControl)
            {
                wrapper = element.AddWrapperInTheUserControl(parentUserControl, wrapper);
            }
            else if (parent is Panel parentPanel)
            {
                wrapper = element.AddWrapperInThePanel(parentPanel, wrapper);
            }
            else if (parent is Border parentBorder)
            {
                wrapper = element.AddWrapperInTheBorder(parentBorder, wrapper);
            }
            else
            {
                throw new NotImplementedException($"Type {parent} is not implemented");
            }

            return wrapper;
        }

        private static void ElementAddFinished(this FrameworkElement element, Action addFinished)
        {
            EventHandler<object> wrapperLayoutUpdatedstroyboard = null;

            wrapperLayoutUpdatedstroyboard = (s, e) =>
            {
                element.LayoutUpdated -= wrapperLayoutUpdatedstroyboard;
                addFinished?.Invoke();
            };
            element.LayoutUpdated += wrapperLayoutUpdatedstroyboard;
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

        public static FrameworkElement AddElementToWrapper(this Panel wrapper, FrameworkElement addElement, Action finished, bool toStart)
        {
            addElement.ElementAddFinished(() =>
            {
                addElement.Visibility = Visibility.Collapsed;
                // TODO : Position out of the screen
                //elementWrapper.TransformInitialize();
                //var fullWidth = elementWrapper.GetFulllWidth(wrapper);
                //var tag = elementWrapper.Tag;
                //((TranslateTransform)elementWrapper.RenderTransform).X = wrapper.ActualWidth > fullWidth ? -wrapper.ActualWidth : -fullWidth;

                finished?.Invoke();
            });

            if (toStart)
                wrapper.Children.Insert(0, addElement);
            else
                wrapper.Children.Add(addElement);


            return addElement;
        }

        //public static Panel AddPinElement(this Panel wrapper, FrameworkElement element, string currentPageKey = null, IReadOnlyCollection<string> forbiddenPageKeys = null)
        //{
        //    var wrapperInfo = GetWrapperInfo(wrapper);

        //    if (!string.IsNullOrEmpty(currentPageKey) && forbiddenPageKeys != null)
        //    {
        //        if (forbiddenPageKeys.FirstOrDefault(x => x.Contains(currentPageKey)) != null)
        //        {
        //            SetElementHidenPosition(wrapper, element);
        //        }
        //    }

        //    if (!(wrapperInfo.Wrapper is Panel wrapperPanel))
        //        throw new ArgumentException($"Wrapper {wrapperInfo.Wrapper} is not a Panel");

        //    // TODO : Temporary
        //    wrapperInfo.Navigation.Pin()

        //    wrapperPanel.Children.Add(element);
        //    wrapperInfo.Pins.Add(new PinInfo(wrapper, element, forbiddenPageKeys));
        //    wrappers[wrapperInfo.Key] = new WrapperInfo(wrapperInfo.Key, wrapperInfo.Wrapper, wrapperInfo.Pins);

        //    return wrapper;
        //}

        //public static (IReadOnlyCollection<FrameworkElement> BlockedPins, IReadOnlyCollection<FrameworkElement> AvailablePins) GetPins(string rootKey, string pageKey)
        //{
        //    if (!wrappers.TryGetValue(rootKey, out WrapperInfo wrapperInfo))
        //        throw new ArgumentException($"[404] Element with \"{rootKey}\" key not found");

        //    List<FrameworkElement> blockedPins = new List<FrameworkElement>();
        //    List<FrameworkElement> availablePins = new List<FrameworkElement>();
        //    foreach (var pinInfo in wrapperInfo.Pins)
        //    {
        //        if (pinInfo.ForbiddenPageKeys.Contains(pageKey))
        //        {
        //            blockedPins.Add(pinInfo.Pin);
        //        }
        //        else
        //        {
        //            availablePins.Add(pinInfo.Pin);
        //        }
        //    }

        //    return (blockedPins, availablePins);
        //}
    }
}
