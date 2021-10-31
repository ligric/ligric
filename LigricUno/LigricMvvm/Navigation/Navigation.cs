using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvm.Navigation
{
    public class Navigation
    {
        private static readonly NavigationService navigationService = new NavigationService();

        public static Task GoTo(string pageName) => Task.Run(() => 
        {
            navigationService.GoTo(pageName);
        });

        public static Task PrerenderPage(object page, string pageName = null, object backPage = null, object nextPage = null) => Task.Run(() =>
        {
            navigationService.PrerenderPage(page, pageName, backPage, nextPage);
        });

        static Navigation()
        {
            navigationService.PageChanged += OnPageChanged;
        }

        private static async void OnPageChanged(object sender, object oldPage, object newPage, PageChangingVectorEnum changingVector)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var mainFrame = Window.Current.Content as Frame;

                if (mainFrame != null)
                {
                    switch (changingVector)
                    {
                        case PageChangingVectorEnum.Back:
                            break;
                        case PageChangingVectorEnum.Next:
                            var page = newPage as FrameworkElement;
                            if (page == null)
                                throw new ArgumentException("New page is null.");

                            mainFrame.Navigate(newPage.GetType(), null, new EntranceNavigationTransitionInfo());

                            break;
                    }
                }
                else
                {
                    throw new ArgumentException("Current Window page wasn't finde.");
                }
            });
        }
    }
}
