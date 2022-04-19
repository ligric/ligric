using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Boards;
using LigricUno.Views.Pages.News;
using LigricUno.Views.Pages.Profile;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace LigricUno.Views.Pages.Login
{
    public class LoginViewModel : DispatchedBindableBase
    {
        private string _email;

        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private RelayCommand _loginLaterCommand;
        public RelayCommand LoginLaterCommand => _loginLaterCommand ?? (_loginLaterCommand = new RelayCommand(LoginLaterMethod));

        public LoginViewModel()
        {
            Navigation.PageRendering += OnPageRendering;
        }


        private int renderingPagesCount;
        private void OnPageRendering(Uno.CheburchayNavigation.Notifications.EventArgs.ElementRenderingEventArgs eventArgs)
        {
            if (eventArgs.Action == Uno.CheburchayNavigation.Notifications.RenderingAction.Rendered)
            {
                renderingPagesCount++;

                if (renderingPagesCount == 14)
                {
                    //Navigation.GoTo(nameof(NewsPage));
                }
            }
        }

        private async void LoginLaterMethod(object parameter)
        {
            Debug.WriteLine("After animation" + Thread.CurrentThread.ManagedThreadId);

            Navigation.GoTo(nameof(LoadingPageTemporary));


            Navigation.PrerenderPage(new NewsPage(), nameof(NewsPage), new NewsViewModel());

            Navigation.PrerenderPage(new SelfProfilePage(), nameof(SelfProfilePage), new SelfProfileViewModel());

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Low, async () =>
            {
                for (int i = 0; i < 20; i++)
                {
                    Navigation.PrerenderPage(new BoardsPage(), nameof(BoardsPage) + i, new BoardsViewModel());
                    await Task.Delay(25);
                }
            });

        }
    }
}
