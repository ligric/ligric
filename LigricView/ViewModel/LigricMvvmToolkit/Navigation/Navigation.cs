using Common.Delegates;
using LigricMvvmToolkit.Extantions;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace LigricMvvmToolkit.Navigation
{
    public class Navigation
    {
        protected static readonly Dictionary<string, NavigationService> navigationServices = new Dictionary<string, NavigationService>()
        {
            { 
                "root", 
                new NavigationService(Window.Current.Content as Frame)
            }
        };

        public static void GoTo(string pageName, string rootKey = "root", object backPage = null, object nextPage = null)
        {
            var navigationService = GetNavigationServiceByRootKey(rootKey);
            navigationService.GoTo(pageName, backPage, nextPage);
        }
        

        public static void PrerenderPage(FrameworkElement page, string pageName = null, object vm = null, string rootKey = "root", string title = null, object backPage = null, object nextPage = null)
        {
            var navigationService = GetNavigationServiceByRootKey(rootKey);
            navigationService.PrerenderPage(page, pageName, vm, title, backPage, nextPage);
        }

        private static NavigationService GetNavigationServiceByRootKey(string rootKey)
        {
            string root = string.Empty;

            if (string.IsNullOrEmpty(rootKey))
            {
                throw new ArgumentNullException($"Root key can't be empty.");
            }
            else
            {
                root = rootKey;
            }

            if (!navigationServices.TryGetValue(root, out NavigationService navigationService))
            {
                throw new ArgumentNullException($"Root key \"{root}\" is not registered or not available. Please use the \"RegisterRoot\" method.");
            }
            return navigationService;
        }

        static Navigation()
        {
            navigationServices["root"].CurrentPageChanged += OnPageChanged;
            navigationServices["root"].ActivePagesChanged += OnActivePagesChanged;
        }

        private static void OnActivePagesChanged(object sender, object rootElement, Common.Enums.ActionCollectionEnum action, PageInfo item)
        {
            if (item?.Page is null)
            {
                throw new ArgumentNullException("Prerender element is null.");
            }

            var prerenderPage = item?.Page as FrameworkElement;
            if (prerenderPage == null)
            {
                throw new ArgumentNullException("Prerender element isn't FrameworkElement.");
            }

            if (item?.ViewModel != null)
            {
                prerenderPage.DataContext = item.ViewModel;
            }


            var rootObject = rootElement as FrameworkElement;
            if (rootObject == null)
            {
                throw new ArgumentNullException("Root element is null or root element isn't FrameworkElement.");
            }

            prerenderPage.Visibility = Visibility.Collapsed;

            rootObject.AddWrapper().AddElementToWrapper(prerenderPage);
        }

        private static async void OnPageChanged(object sender, object rootElement, PageInfo oldPageInfo, PageInfo newPageInfo, PageChangingVectorEnum changingVector)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                var root = rootElement as FrameworkElement;
                var oldPage = oldPageInfo?.Page as FrameworkElement;
                var newPage = newPageInfo?.Page as FrameworkElement;

                if (root is null)
                {
                    root = rootElement is DependencyObject rootDependencyObject ? rootDependencyObject.GetVisualChild<Panel>() : null;
                    if (root is null)
                        throw new ArgumentException("Cannot change the page because root element does't contain a Panel element.");
                }
                if (oldPage is null)
                {
                    if (changingVector == PageChangingVectorEnum.Back)
                    {
                        throw new ArgumentException("Cannot change the page because old page is null");
                    }
                    oldPage = root;
                }
                if (newPage is null)
                {
                    throw new ArgumentException("Cannot change the page because new page is null");
                }
                newPage.Visibility = Visibility.Collapsed;

                switch (changingVector)
                {
                    case PageChangingVectorEnum.Back:
                        break;
                    case PageChangingVectorEnum.Next:
                        root.AddWrapper().GetTrainAnimationStrouyboard(oldPage, newPage, 300).Begin();
                        break;
                }
            });
        }
    }
}
