using LigricMvvmToolkit.BaseMvvm;
using LigricMvvmToolkit.Navigation;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricUno.Views.Pins
{
    public class NavigationHeaderItemsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate News { get; set; }
        public DataTemplate Profile { get; set; }
        public DataTemplate Settings { get; set; }
        public DataTemplate Messages { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is string key)
            {
                return SelectTemplateCore(key);
            }

            throw new System.NotImplementedException($"Uknown DataTemplate parameter. \nType: {nameof(NavigationHeaderItemsDataTemplateSelector)}\nParametr: {item}");
        }

        private DataTemplate SelectTemplateCore(string key)
        {
            if (string.Equals(key, nameof(News)))
                return News;

            if (string.Equals(key, nameof(Profile)))
                return Profile;

            if (string.Equals(key, nameof(Settings)))
                return Settings;

            if (string.Equals(key, nameof(Messages)))
                return Messages;

            throw new System.NotImplementedException($"Uknown DataTemplate key. \nType: {nameof(NavigationHeaderItemsDataTemplateSelector)}\nKey: {key}");
        }
    }

    public class NavigationHeaderOptionItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ProfileSettings { get; set; }
        public DataTemplate BoardSettings { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is string key)
            {
                return SelectTemplateCore(key);
            }

            throw new System.NotImplementedException($"Uknown DataTemplate parameter. \nType: {nameof(NavigationHeaderItemsDataTemplateSelector)}\nParametr: {item}");
        }

        private DataTemplate SelectTemplateCore(string key)
        {
            if (string.Equals(key, nameof(ProfileSettings)))
                return ProfileSettings;

            if (string.Equals(key, nameof(BoardSettings)))
                return BoardSettings;

            throw new System.NotImplementedException($"Uknown DataTemplate key. \nType: {nameof(NavigationHeaderItemsDataTemplateSelector)}\nKey: {key}");
        }
    }


    public class NavigationHeaderViewData : DispatchedBindableBase
    {
        public enum Mode
        {
            Left,
            Right,
            Bottom,
            Top
        }

        private Mode _currentMode;
        private double? _elementsTemplateWidth, _elementsTemplateHeight;

        public double? ElementsTemplateWidth { get => _elementsTemplateWidth; set => SetProperty(ref _elementsTemplateWidth, value); }
        public double? ElementsTemplateHeight { get => _elementsTemplateHeight; set => SetProperty(ref _elementsTemplateHeight, value); }
        public Mode CurrentMode {
            get => _currentMode; 
            set => SetProperty(ref _currentMode, value); }
    }

    public sealed partial class NavigationMenu : UserControl
    {
        private bool useAnimation = false;

        public NavigationHeaderViewData HeaderData { get; } = new NavigationHeaderViewData() { ElementsTemplateHeight = 50 };

        public NavigationMenu()
        {
            this.InitializeComponent();
            LayoutUpdated += OnLayoutUpdated;
            menu.ExpanderSideChanged += OnMenuSideChanged;
            ////// TODO : TEMPRARY
            stackPanel.LayoutUpdated += OnStackPanelLayoutUpdated;
            SetSideSettings(menu.ExpanderSide);
            Navigation.PinsVisibilityChanged += OnPinsVisibilityChanged;
        }

        private void OnPinsVisibilityChanged(IReadOnlyCollection<string> oldKeys, IReadOnlyCollection<string> newKeys)
        {
            if (newKeys.FirstOrDefault(x => x.Equals(nameof(NavigationMenu))) != null)
            {
                useAnimation = false;
                SetExpanderSideFromScreenAspectRatio();
            }
        }

        private void OnLayoutUpdated(object sender, object e)
        {
            SetExpanderSideFromScreenAspectRatio();
        }

        private void SetExpanderSideFromScreenAspectRatio()
        {
            if (ActualHeight == 0 || ActualWidth == 0)
                return;

            if (ActualHeight / 3.0 >= ActualWidth / 2.0)
            {
                if (useAnimation)
                {
                    menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Bottom;
                }
                else
                {
                    menu.SetSideWithoutAnimation(LigricBoardCustomControls.Menus.ExpanderSide.Bottom);
                }
            }
            else
            {
                if (useAnimation)
                {
                    menu.ExpanderSide = LigricBoardCustomControls.Menus.ExpanderSide.Left;
                }
                else
                {
                    menu.SetSideWithoutAnimation(LigricBoardCustomControls.Menus.ExpanderSide.Left);
                }
            }

            useAnimation = true;
        }

        ////// TODO : TEMPRARY
        private void OnStackPanelLayoutUpdated(object sender, object e)
        {
            double buttonWidthArea = navigationListHeader.ActualWidth * 1.1;
            double buttonHeightArea = navigationListHeader.ActualHeight * 1.5;

            if (stackPanel.Orientation == Orientation.Horizontal)
            {
                HeaderData.ElementsTemplateWidth = buttonHeightArea;
                HeaderData.ElementsTemplateHeight = null;
                HeaderData.CurrentMode = NavigationHeaderViewData.Mode.Bottom;
            }
            else
            {
                HeaderData.ElementsTemplateWidth = null;
                HeaderData.ElementsTemplateHeight = buttonWidthArea;
                HeaderData.CurrentMode = NavigationHeaderViewData.Mode.Left;
            }
        }

        private void OnMenuSideChanged(object sender, LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            SetSideSettings(newSide);
        }

        private void SetSideSettings(LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            if (newSide == LigricBoardCustomControls.Menus.ExpanderSide.Left)
            {
                VisualStateManager.GoToState(this, "ExpanderHeaderSettingsForLeftSide", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "ExpanderHeaderSettingsForBottomSide", false);
            }
        }

        private void OnContentClicked(object sender, RoutedEventArgs e)
        {
            menu.ExpanderState = LigricBoardCustomControls.Menus.ExpanderState.Collapsed;
        }
    }
}
