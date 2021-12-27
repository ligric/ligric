using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace LigricMvvmToolkit.Navigation
{
    public partial class Navigation
    {
        public static void RegisterRootElement()
        {
            throw new NotImplementedException();
        }

        public static void GoTo(string pageName, string rootKey = "root", object backPage = null, object nextPage = null)
        {
            var navigationService = GetNavigationServiceByRootKey(rootKey);
            navigationService.GoTo(pageName, backPage, nextPage);
        }

        public static void PrerenderPage(FrameworkElement page, string pageName = null, object vm = null, string rootKey = "root", string title = null, object backPage = null, object nextPage = null)
        {
            var navigationService = GetNavigationServiceByRootKey(rootKey);
            navigationService.PrerenderPage(page, pageName, vm, title, backPage, nextPage);
        }

        /// <summary>
        /// Anchors the element above the page. This can be used, for example, to pin a menu.
        /// </summary>
        /// <param name="frontElement">An element that will always appear in front of the screen.</param>
        /// <param name="rootKey"><see cref="RegisterRootElement"/></param>
        /// <param name="forbiddenPageKey">Keys of the pages on which the element will not be displayed.</param>
        public static void PinFrontElement(FrameworkElement frontElement, string rootKey = "root", IReadOnlyCollection<string> forbiddenPageKey = null)
        {
            var navigationService = GetNavigationServiceByRootKey(rootKey);
            var root = navigationService.RootElement;
            ((FrameworkElement)root).AddWrapper(rootKey).AddPinElement(frontElement);
        }


        private static NavigationService GetNavigationServiceByRootKey(string rootKey)
        {
            string root = string.Empty;

            if (string.IsNullOrEmpty(rootKey))
            {
                throw new ArgumentNullException($"Root key can't be empty.");
            }
            else
            {
                root = rootKey;
            }

            if (!navigationServices.TryGetValue(root, out NavigationService navigationService))
            {
                throw new ArgumentNullException($"Root key \"{root}\" is not registered or not available. Please use the \"RegisterRoot\" method.");
            }
            return navigationService;
        }
    }
}
