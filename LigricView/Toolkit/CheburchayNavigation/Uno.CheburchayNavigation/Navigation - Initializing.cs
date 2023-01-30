using CheburchayNavigation.Native;
using System;
using System.Collections.Generic;
using Uno.CheburchayNavigation.InfoModels;
using Uno.CheburchayNavigation.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    public partial class Navigation
    {
        private static string DEFAULT_ROOT_NAME = "root";

        private static readonly HashSet<string> renderingPages = new HashSet<string>();
        private static readonly HashSet<string> renderingWrappers = new HashSet<string>();

        private static readonly Dictionary<string, WrapperInfo> navigations = new Dictionary<string, WrapperInfo>();

        public static event Action<string> PageChanged;
        public static event ElementRenderingHandler PageRendering;
        public static event ElementRenderingHandler WrapperRendering;

        static Navigation()
        {
            var defaultNavigation = new NavigationService(DEFAULT_ROOT_NAME);

            Panel defaultWrapper = null;

            SetWrapperRendering(DEFAULT_ROOT_NAME);

            animationFinished += OnAnimationFinished;

            WaitWrapperRendering(DEFAULT_ROOT_NAME, () =>
            {
                navigations.Add(DEFAULT_ROOT_NAME, new WrapperInfo(DEFAULT_ROOT_NAME, defaultWrapper, defaultNavigation));

                defaultNavigation.PageExistenceChanged += OnPageExistenceChanged;

                defaultNavigation.CurrentPageChanged += OnCurrentPageChanged;

                defaultNavigation.PinsExistenceChanged += OnPinsExistenceChanged;

                defaultNavigation.PinsDirectionChanged += OnPinsDirectionChanged;

                defaultNavigation.PinVisibilityChanged += OnPinVisibilityChanged;
            });

            defaultWrapper = (Window.Current.Content as Frame).AddWrapper(() => 
            {
                FinishWrapperRendering(DEFAULT_ROOT_NAME);
            });
        }
    }
}
