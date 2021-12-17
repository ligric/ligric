using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvmToolkit.Navigation
{
    public class Navigation
    {
        private static readonly Dictionary<string, NavigationService> navigationServices = new Dictionary<string, NavigationService>()
        {
            { 
                "root", 
                new NavigationService(Window.Current.Content as FrameworkElement)
            }
        };

        public static void GoTo(string pageName, string rootKey = null, object backPage = null, object nextPage = null)
        {
            var navigationService = GetNavigationServiceByRootName(rootKey);
            navigationService.GoTo(pageName, backPage, nextPage);
        }
        

        public static void PrerenderPage(object page, string pageName = null, string rootKey = null, string title = null, object backPage = null, object nextPage = null)
        {
            var navigationService = GetNavigationServiceByRootName(rootKey);
            navigationService.PrerenderPage(page, pageName, title, backPage, nextPage);
        }

        private static NavigationService GetNavigationServiceByRootName(string rootKey)
        {
            string root = string.Empty;

            if (rootKey is null)
            {
                root = "root";
            }
            else
            {
                if (string.IsNullOrEmpty(rootKey))
                {
                    throw new ArgumentNullException($"rootName can't be empty. MethodName: {nameof(GoTo)}");
                }
                else
                {
                    root = rootKey;
                }
            }

            if (!navigationServices.TryGetValue(root, out NavigationService navigationService))
            {
                throw new ArgumentNullException($"Key {root} is not registered or not available. Please use the RegisterRoot method.");
            }
            return navigationService;
        }

        static Navigation()
        {
            navigationServices["root"].CurrentPageChanged += OnPageChanged;
        }

        private static async void OnPageChanged(object sender, object rootElement, PageInfo oldPage, PageInfo newPageInfo, PageChangingVectorEnum changingVector)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var root = rootElement as FrameworkElement;
                //var element = oldPage.Page as FrameworkElement;
                //var parent = element.Parent as Grid;


                if (root != null)
                {
                    switch (changingVector)
                    {
                        case PageChangingVectorEnum.Back:
                            break;
                        case PageChangingVectorEnum.Next:
                            var page = newPageInfo.Page as FrameworkElement;
                            if (page == null)
                                throw new ArgumentException("New page is null.");

                            //mainFrame.Navigate(newPageInfo.Page.GetType(), null, new EntranceNavigationTransitionInfo());
                            break;
                    }
                }
                else
                {
                    throw new ArgumentException("You cannot change the page for.");
                }
            });
        }
    }
}
