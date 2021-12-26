using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricMvvmToolkit.Navigation
{
    public partial class Navigation
    {
        protected static readonly Dictionary<string, NavigationService> navigationServices = new Dictionary<string, NavigationService>()
        {
            {
                "root",
                new NavigationService(Window.Current.Content as Frame)
            }
        };

        static Navigation()
        {
            navigationServices["root"].CurrentPageChanged += OnPageChanged;
            navigationServices["root"].ActivePagesChanged += OnActivePagesChanged;
        }
    }
}
