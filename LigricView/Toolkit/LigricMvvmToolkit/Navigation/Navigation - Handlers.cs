using LigricMvvmToolkit.Animations;
using LigricMvvmToolkit.Extensions;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

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

                rootObject.AddWrapper().AddElementToWrapper(prerenderPage);
            });
        }

        private static async void OnPageChanged(object sender, object rootElement, PageInfo oldPageInfo, PageInfo newPageInfo, PageChangingVectorEnum changingVector, int? index)
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
                
                switch (changingVector)
                {
                    case PageChangingVectorEnum.Back:
                        break;
                    case PageChangingVectorEnum.Next:
                        MoveNext(root, oldPage, newPage, newPageInfo, index);
                        break;
                }
            });
        }

        private static SyncAnimations syncAnimations = new SyncAnimations();
        private static void MoveNext(FrameworkElement root, FrameworkElement oldPage, FrameworkElement newPage, PageInfo newPageInfo, int? syncIndex)
        {
            IReadOnlyCollection<FrameworkElement> blockedPins = ElementsSeparatorExtensions.GetBlockedPins("root", newPageInfo.PageKey);

            syncAnimations.ExecuteAnimation((int)syncIndex, () => root.AddWrapper().GetTrainAnimationStrouyboard(oldPage, newPage, 10_000), () => 
            {
                var olddPageParent = oldPage.Parent as FrameworkElement;
                olddPageParent.Visibility = Visibility.Collapsed;
            });

            foreach (var item in blockedPins)
            {
                root.AddWrapper().GetTrainAnimationStrouyboard(firstVisibileElement: item, timeMilliseconds: 200);
            }
        }
    }
}
