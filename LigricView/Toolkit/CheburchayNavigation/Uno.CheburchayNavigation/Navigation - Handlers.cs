using CheburchayNavigation.Native.InfoModels;
using CheburchayNavigation.Native.Interfaces;
using CheburchayNavigation.Native.Notifications.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Uno.CheburchayNavigation;
using Uno.CheburchayNavigation.InfoModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvmToolkit.Navigation
{
    public delegate void ActionActivePagesChangedHandler(string pageKey);
    public delegate void PinsVisiblilityChangedHandler(IReadOnlyCollection<string> oldKeys, IReadOnlyCollection<string> newKeys);

    public partial class Navigation
    {
        public static event ActionActivePagesChangedHandler ActivePagesChanged;
        public static event PinsVisiblilityChangedHandler PinsVisibilityChanged;

        private static SyncAnimations syncPagesAnimation = new SyncAnimations();

        private static int syncPageAnimations = 0;

        private static void OnPageExistenceChanged(object sender, PageExistenceAction action, PageInfo pageInfo)
        {
            if (pageInfo.Element is null)
                throw new NullReferenceException("Prerender element is null.");

            if (!(sender is INavigationService navigation))
                throw new NullReferenceException($"Sender {sender} is not a INavigationService.");

            if (!(pageInfo.Element is FrameworkElement prerenderPage))
                throw new ArgumentNullException("Prerender element is not a FrameworkElement.");

            if (pageInfo.ViewModel != null)
                prerenderPage.DataContext = pageInfo.ViewModel;

            if (!navigations.TryGetValue(navigation.RootKey, out WrapperInfo wrapper))
                throw new NullReferenceException("Wrapper not found.");

            // TODO : Temporary realization. Can be any types. Should be extension for finding type method;
            if (!(wrapper.Wrapper is Panel wrapperPanel))
                throw new NullReferenceException($"Wrapper {wrapper} is not a Panel.");

            ExecuteWhenAnimationFinished(() => 
            {
                SetPageRendering(pageInfo.Key);

                wrapperPanel.AddElementToWrapper(prerenderPage, () =>
                {
                    FinishPageRendering(pageInfo.Key);
                    ActivePagesChanged?.Invoke(pageInfo.Key);
                }, true);
            });
        }

        private static void OnCurrentPageChanged(object sender, ElementDirectionChangedEventArgs<PageInfo> eventArgs, ElementsDirectionChangedEventArgs<PinInfo> eventArgs1)
        {
            if (eventArgs is null)
                throw new NullReferenceException("Agrs is null.");

            if (!(sender is INavigationService navigation))
                throw new NullReferenceException($"Sender {sender} is not a INavigationService.");

            if (!navigations.TryGetValue(navigation.RootKey, out WrapperInfo wrapper))
                throw new NullReferenceException("Wrapper not found.");

            if (!(wrapper.Wrapper is Panel wrapperPanel))
                throw new NullReferenceException($"Wrapper {wrapper} is not a Panel.");


            var syncIndex = syncPageAnimations++;

            var elements = new List<(IModelInfo OldElement, IModelInfo NewElement)>()
            {
                (eventArgs.OldElement, eventArgs.NewElement)
            };

            foreach (var item in eventArgs1.NewElements)
                elements.Add((null, item));

            foreach (var item in eventArgs1.OldElements)
                elements.Add((item, null));

            ExecuteWhenAnimationFinished(() =>
            {
                WaitPageRendering(eventArgs.NewElement.Key, () =>
                {
                    if (eventArgs.Direction == CheburchayNavigation.Native.Enums.ElementDirection.Next)
                        ExecuteElementsMoveAnimation(wrapperPanel, elements, syncIndex, () => PageChanged?.Invoke(eventArgs.NewElement.Key));
                    else
                        throw new NotImplementedException("Actually back doesn't work.");
                });
            });
        }

        private static void MoveNext(Panel wrapper, PageInfo oldPageInfo, PageInfo newPageInfo, int syncIndex)
        {
            if (!(newPageInfo.Element is FrameworkElement newPage))
                throw new NullReferenceException($"Sender new page {newPageInfo.Element} is not a FrameworkElement.");

            FrameworkElement oldPage = null;

            if (oldPageInfo != null)
            {
                if (!(oldPageInfo?.Element is FrameworkElement oldPageOut))
                {
                    throw new NullReferenceException($"Sender {oldPageInfo.Element} is not a FrameworkElement.");
                }
                else
                {
                    oldPage = oldPageOut;
                }
            }

            RaiseAnimationFinishedAction(true);
            syncPagesAnimation.ExecuteAnimation(syncIndex, () => wrapper.GetTrainAnimationStrouyboard(oldPage, newPage, 200), () => 
            {
                if (oldPage != null)
                    oldPage.Visibility = Visibility.Collapsed;

                RaiseAnimationFinishedAction(false);
            });
        }

        private static void OnPinsExistenceChanged(object sender, PinInfo pinInfo, PinsAction action)
        {
            if (pinInfo.Element is null)
                throw new NullReferenceException("Prerender element is null.");

            if (!(sender is INavigationService navigation))
                throw new NullReferenceException($"Sender {sender} is not a INavigationService.");

            if (!navigations.TryGetValue(navigation.RootKey, out WrapperInfo wrapper))
                throw new NullReferenceException("Wrapper not found.");

            if (!(pinInfo.Element is FrameworkElement prerenderPage))
                throw new ArgumentNullException("Prerender element is not a FrameworkElement.");

            if (pinInfo.ViewModel != null)
                prerenderPage.DataContext = pinInfo.ViewModel;

            if (!(wrapper.Wrapper is Panel wrapperPanel))
                throw new NullReferenceException($"Wrapper {wrapper} is not a Panel.");

            wrapperPanel.AddElementToWrapper(prerenderPage, () =>
            {
                //ActivePagesChanged?.Invoke(pinInfo.PinKey);
            }, false);
        }

        private static void OnPinsDirectionChanged(object sender, ElementsDirectionChangedEventArgs<PinInfo> eventArgs)
        {
            // TODO : Temporary

            if (eventArgs is null)
                throw new NullReferenceException("Agrs is null.");

            if (!(sender is INavigationService navigation))
                throw new NullReferenceException($"Sender {sender} is not a INavigationService.");

            if (!navigations.TryGetValue(navigation.RootKey, out WrapperInfo wrapper))
                throw new NullReferenceException("Wrapper not found.");

            if (!(wrapper.Wrapper is Panel wrapperPanel))
                throw new NullReferenceException($"Wrapper {wrapper} is not a Panel.");

 
            foreach (var item in eventArgs.OldElements)
            {
            }

            foreach (var item in eventArgs.NewElements)
            {
                wrapperPanel.GetTrainAnimationStrouyboard(null, (FrameworkElement)item.Element, 200);
            }

            PinsVisibilityChanged?.Invoke(new ReadOnlyCollection<string>(eventArgs.OldElements.Select(x => x.Key).ToArray()),
                new ReadOnlyCollection<string>(eventArgs.NewElements.Select(x => x.Key).ToArray()));
        }

        private static void ExecuteElementsMoveAnimation(Panel wrapper, IList<(IModelInfo OldElement, IModelInfo NewElement)> elements, int syncIndex, Action finished)
        {
            RaiseAnimationFinishedAction(true);

            Dictionary<int, Func<Storyboard>> storyboards = new Dictionary<int, Func<Storyboard>>();

            for (int i = 0; i < elements.Count; i++)
            {
                var element = elements[i];
                var old = element.OldElement?.Element as FrameworkElement;
                var @new = element.NewElement?.Element as FrameworkElement;
                storyboards.Add(i, () => wrapper.GetTrainAnimationStrouyboard(old, @new, 200));
            }

            var storyboardsReadOnly = new ReadOnlyDictionary<int, Func<Storyboard>>(storyboards);
            int index = 0;
            syncPagesAnimation.ExecuteAnimations(syncIndex, storyboardsReadOnly, (int key) =>
            {
                index++;
                var old = elements[key].OldElement?.Element;
                if (old != null)
                {
                    ((FrameworkElement)old).Visibility = Visibility.Collapsed;
                }

                if (index == storyboardsReadOnly.Count)
                {
                    RaiseAnimationFinishedAction(false);

                    finished?.Invoke();
                }
            });
        }

        private static void OnPinVisibilityChanged(object sender, PinInfo pinInfo, CheburchayNavigation.Native.Enums.SwitchState newState)
        {
            if (!(pinInfo.Element is FrameworkElement pin))
                throw new ArgumentException($"Object {pinInfo.Element} is not FrameworkElement.");

            pin.Visibility = newState == CheburchayNavigation.Native.Enums.SwitchState.Visible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
