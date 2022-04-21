using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using LigricMvvmToolkit.RelayCommand;
using LigricUno.Views.Pages.Boards;
using LigricUno.Views.Pages.News;
using LigricUno.Views.Pages.Profile;
using System;
using System.Collections.Generic;
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

                if (prerenderPageActions.Count > 0)
                {
                    var index = prerenderPageActions[0].Index;
                    var action = prerenderPageActions[0].Action;
                    prerenderPageActions.RemoveAt(0);
                    action.Invoke(index);
                }
                
                if (renderingPagesCount == 14)
                {
                    //Navigation.GoTo(nameof(NewsPage));
                }
            }
        }

        private readonly List<(Action<int> Action, int Index)> prerenderPageActions = new List<(Action<int> Action, int Index)>();

        private void LoginLaterMethod(object parameter)
        {
            Debug.WriteLine("After animation" + Thread.CurrentThread.ManagedThreadId);

            Navigation.GoTo(nameof(LoadingPageTemporary));

            Navigation.PrerenderPage(new NewsPage(), nameof(NewsPage), new NewsViewModel());
            Navigation.PrerenderPage(new SelfProfilePage(), nameof(SelfProfilePage), new SelfProfileViewModel());

            for (int i = 1; i < 20; i++)
            {
                prerenderPageActions.Add(((int j) =>
                {
                    Navigation.PrerenderPage(new BoardsPage(), nameof(BoardsPage) + j, new BoardsViewModel());
                }, i));
            }

            Navigation.PrerenderPage(new BoardsPage(), nameof(BoardsPage) + 0, new BoardsViewModel());

        }
    }
}
