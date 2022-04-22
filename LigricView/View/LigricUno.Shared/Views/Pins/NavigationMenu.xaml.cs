using LigricMvvmToolkit.Navigation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace LigricUno.Views.Pins
{
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

            double widthArea = (rect.Width - userImage.Margin.Left - userImage.Margin.Right);
            widthArea = widthArea < 0 ? 0 : widthArea;

            double heightArea = rect.Height - userImage.Margin.Top - userImage.Margin.Bottom;
            heightArea = heightArea < 0 ? 0 : heightArea;

            double buttonWidthArea = (rect.Width - boards.Margin.Left - boards.Margin.Right) * 1.1;
            double buttonHeightArea = (rect.Height - boards.Margin.Top - boards.Margin.Bottom) * 1.5;

            if (stackPanel.Orientation == Orientation.Horizontal)
            {
                //if (userImage.Visibility != Visibility.Collapsed)
                userImage.Width = heightArea;
                news.Width = buttonHeightArea;
                profile.Width = buttonHeightArea;
                boards.Width = buttonHeightArea;
                settings.Width = buttonHeightArea;
            }
            else
            {
                userImage.Height = widthArea;
                news.Height = buttonWidthArea;
                profile.Height = buttonWidthArea;
                boards.Height = buttonWidthArea;
                settings.Height = buttonWidthArea;
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
                VisualStateManager.GoToState(this, "ExpanderSettingsForLeftSidee", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "ExpanderSettingsForBottomSidee", false);
            }
        }
    }
}
