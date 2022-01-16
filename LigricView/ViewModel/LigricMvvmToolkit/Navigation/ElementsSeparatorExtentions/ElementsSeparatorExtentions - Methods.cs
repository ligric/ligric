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

            wrapper.Children.Add(element);

            parent.Child = wrapper;

            return wrapper;
        }
        #endregion

        public static FrameworkElement AddElementToWrapper(this Panel wrapper, FrameworkElement addElement)
        {
            var wrapperInfo = GetWrapperInfo(wrapper);

            var count = wrapper.Children.Count;

            //addElement.Clip = new RectangleGeometry()
            //{
            //    Rect = new Rect(new Point(0, 0),
            //                    new Point(wrapper.ActualWidth, wrapper.Height))
            //};

            //wrapper.SizeChanged += (s, e) =>
            //{
            //    addElement.Clip = new RectangleGeometry()
            //    {
            //        Rect = new Rect(new Point(0, 0),
            //        new Point(e.NewSize.Width, e.NewSize.Height))
            //    };
            //};

            wrapper.Children.Insert(0/*count - wrapperInfo.Pins.Count*/, addElement);

            addElement.TransformInitialize();
            ((TranslateTransform)addElement.RenderTransform).X = -wrapper.ActualWidth;
            wrapper.SizeChanged += (s, e) => ((TranslateTransform)addElement.RenderTransform).X = -e.NewSize.Width;

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
