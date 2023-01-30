using CheburchayNavigation.Native;
using CheburchayNavigation.Native.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Uno.CheburchayNavigation.InfoModels;
using Uno.CheburchayNavigation.Notifications;
using Uno.CheburchayNavigation.Notifications.EventArgs;
using Windows.UI.Xaml;

namespace LigricMvvmToolkit.Navigation
{
    public partial class Navigation
    {
        public static void GoTo(string pageKey, string rootKey = "root")
        {
            WaitWrapperRendering(rootKey, () =>
            {
                GetNavigationWrapperByRootKey(rootKey).Navigation.SetCurrentPage(pageKey);
            });
        }

        public static void GoTo(FrameworkElement page, string pageKey, object vm = null, string rootKey = "root", object backPage = null, object nextPage = null)
        {
            WaitWrapperRendering(rootKey, () =>
            {
                GetNavigationWrapperByRootKey(rootKey).Navigation.SetCurrentPage(page, pageKey, vm, backPage, nextPage);
            });
        }

        public static void PrerenderPage(FrameworkElement page, string pageKey, object vm = null, string rootKey = "root", object backPage = null, object nextPage = null)
        {
            WaitWrapperRendering(rootKey, () =>
            {
                GetNavigationWrapperByRootKey(rootKey).Navigation.PrerenderPage(page, pageKey, vm, backPage, nextPage);
            });
        }

        public static void RegisterRootElement()
            => throw new NotImplementedException();

        public static IEnumerable<string> GetPageKeys(string rootKey = "root")
        {
            var pages = GetNavigationWrapperByRootKey(rootKey).Navigation.Pages;

            IList<string> pagesOut = new List<string>();

            foreach (var item in pages)
                pagesOut.Add(item.Key);

            return pagesOut;
        }

        public static string GetCurrentPageKey(string rootKey = "root")
            => GetNavigationWrapperByRootKey(rootKey).Navigation.CurrentPage.Key;

        /// <summary>
        /// Anchors the element above the page. This can be used, for example, to pin a menu.
        /// </summary>
        /// <param name="frontElement">An element that will always appear in front of the screen.</param>
        /// <param name="rootKey"><see cref="RegisterRootElement"/></param>
        /// <param name="forbiddenPageKey">Keys of the pages on which the element will not be displayed.</param>
        public static void Pin(FrameworkElement frontElement, string pinKey, IEnumerable<string> forbiddenPageKeys = null, object viewModel = null, bool isVisible = true, string rootKey = "root")
        {
            WaitWrapperRendering(rootKey, () =>
            {
                GetNavigationWrapperByRootKey(rootKey).Navigation.Pin(frontElement, pinKey, forbiddenPageKeys, viewModel, isVisible ? SwitchState.Visible : SwitchState.Collapsed);
            });
        }

        public static void TurnOffPin(string pinKey, string rootKey = "root")
        {
            WaitWrapperRendering(rootKey, () =>
            {
                GetNavigationWrapperByRootKey(rootKey).Navigation.TurnOffPin(pinKey);
            });
        }

        public static void TurnOnPin(string pinKey, string rootKey = "root")
        {
            WaitWrapperRendering(rootKey, () =>
            {
                GetNavigationWrapperByRootKey(rootKey).Navigation.TurnOnPin(pinKey);
            });
        }


        private static WrapperInfo GetNavigationWrapperByRootKey(string rootKey)
        {
            if (string.IsNullOrEmpty(rootKey))
                throw new ArgumentNullException($"Root key can't be empty.");

            if (!navigations.TryGetValue(rootKey, out WrapperInfo wrapperInfo))
                throw new ArgumentNullException($"Root key \"{rootKey}\" is not registered or not available. Please use the \"RegisterRoot\" method.");

            return wrapperInfo;
        }







        private static List<Action> waitingAnimationActions = new List<Action>();
        private static bool isBusy = false;

        private static event Action animationFinished;

        private static void ExecuteWhenAnimationFinished(Action action)
        {
            lock (((ICollection)waitingAnimationActions).SyncRoot)
            {
                if (!isBusy && waitingAnimationActions.Count == 0)
                    action?.Invoke();
                else
                    waitingAnimationActions.Add(action);
            }
        }

        private static void OnAnimationFinished()
        {
            lock (((ICollection)waitingAnimationActions).SyncRoot)
            {
                var removeElements = new List<Action>();
                foreach (var item in waitingAnimationActions)
                {
                    if (isBusy)
                    {
                        break;
                    }

                    item();
                    removeElements.Add(item);
                }

                foreach (var item in removeElements)
                {
                    waitingAnimationActions.Remove(item);
                }
            }
        }

        private static void RaiseAnimationFinishedAction(bool isBusyNew)
        {
            isBusy = isBusyNew;

            if (!isBusyNew)
            {
                animationFinished?.Invoke();
            }
        }










        private static void WaitPageRendering(string pageKey, Action rendered)
        {
            if (!renderingPages.Contains(pageKey))
            {
                rendered?.Invoke();
                return;
            }

            ElementRenderingHandler elementRendering = null;

            elementRendering = (e) =>
            {
                if (e.Action == RenderingAction.Rendered && string.Equals(e.ElementKey, pageKey))
                {
                    PageRendering -= elementRendering;

                    rendered?.Invoke();
                }
            };
            PageRendering += elementRendering;
        }
        private static void SetPageRendering(string pageKey)
        {
            renderingPages.Add(pageKey);
            PageRendering?.Invoke(new ElementRenderingEventArgs(pageKey, RenderingAction.Rendering));
        }
        private static void FinishPageRendering(string pageKey)
        {
            renderingPages.Remove(pageKey);
            PageRendering?.Invoke(new ElementRenderingEventArgs(pageKey, RenderingAction.Rendered));
        }


        private static void WaitWrapperRendering(string wrapperKey, Action rendered)
        {
            if (!renderingWrappers.Contains(wrapperKey))
            {
                rendered?.Invoke();
                return;
            }

            ElementRenderingHandler elementRendering = null;

            elementRendering = (e) =>
            {
                if (e.Action == RenderingAction.Rendered && string.Equals(e.ElementKey, wrapperKey))
                {
                    WrapperRendering -= elementRendering;
                    rendered?.Invoke();
                }
            };
            WrapperRendering += elementRendering;
        }
        private static void SetWrapperRendering(string wrapperKey)
        {
            renderingWrappers.Add(wrapperKey);
            WrapperRendering?.Invoke(new ElementRenderingEventArgs(wrapperKey, RenderingAction.Rendering));
        }
        private static void FinishWrapperRendering(string wrapperKey)
        {
            renderingWrappers.Remove(wrapperKey);
            WrapperRendering?.Invoke(new ElementRenderingEventArgs(wrapperKey, RenderingAction.Rendered));
        }
    }
}
