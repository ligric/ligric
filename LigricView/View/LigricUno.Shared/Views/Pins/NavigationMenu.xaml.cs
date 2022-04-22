using LigricMvvmToolkit.Navigation;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricUno.Views.Pins
{
    public class NavigationItemsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate News { get; set; }
        public DataTemplate Profile { get; set; }
        public DataTemplate Settings { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is string key)
            {
                return SelectTemplateCore(key);
            }

            throw new System.NotImplementedException($"Uknown DataTemplate parameter. \nType: {nameof(NavigationItemsDataTemplateSelector)}\nParametr: {item}");
        }

        private DataTemplate SelectTemplateCore(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new System.NullReferenceException(key);

            if (string.Equals(key, nameof(News)))
                return News;

            if (string.Equals(key, nameof(Profile)))
                return Profile;

            if (string.Equals(key, nameof(Settings)))
                return Settings;

            throw new System.NotImplementedException($"Uknown DataTemplate key. \nType: {nameof(NavigationItemsDataTemplateSelector)}\nKey: {key}");
        }
    }

    public sealed partial class NavigationMenu : UserControl
    {
        private bool useAnimation = false;

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
            Rect rect = LayoutInformation.GetLayoutSlot(stackPanel);

            //double widthArea = (rect.Width - userImage.Margin.Left - userImage.Margin.Right);
            //widthArea = widthArea < 0 ? 0 : widthArea;

            //double heightArea = rect.Height - userImage.Margin.Top - userImage.Margin.Bottom;
            //heightArea = heightArea < 0 ? 0 : heightArea;

            //double buttonWidthArea = (rect.Width - boards.Margin.Left - boards.Margin.Right) * 1.1;
            //double buttonHeightArea = (rect.Height - boards.Margin.Top - boards.Margin.Bottom) * 1.5;

            //if (stackPanel.Orientation == Orientation.Horizontal)
            //{
            //    //if (userImage.Visibility != Visibility.Collapsed)
            //    userImage.Width = heightArea;
            //    news.Width = buttonHeightArea;
            //    profile.Width = buttonHeightArea;
            //    boards.Width = buttonHeightArea;
            //    settings.Width = buttonHeightArea;
            //}
            //else
            //{
            //    userImage.Height = widthArea;
            //    news.Height = buttonWidthArea;
            //    profile.Height = buttonWidthArea;
            //    boards.Height = buttonWidthArea;
            //    settings.Height = buttonWidthArea;
            //}
        }

        private void OnMenuSideChanged(object sender, LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            SetSideSettings(newSide);
        }

        private void SetSideSettings(LigricBoardCustomControls.Menus.ExpanderSide newSide)
        {
            //if (newSide == LigricBoardCustomControls.Menus.ExpanderSide.Left)
            //{
            //    VisualStateManager.GoToState(this, "ExpanderSettingsForLeftSidee", false);
            //}
            //else
            //{
            //    VisualStateManager.GoToState(this, "ExpanderSettingsForBottomSidee", false);
            //}
        }
    }
}
