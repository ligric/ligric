using LigricMvvmToolkit.Extantions;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    public partial class Navigation
    {
        private static async void OnActivePagesChanged(object sender, object rootElement, Common.Enums.ActionCollectionEnum action, PageInfo item)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
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
            });
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
