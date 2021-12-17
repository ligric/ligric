using Common.Delegates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigricMvvmToolkit.Navigation
{
    public enum PageChangingVectorEnum
    {
        Back,
        Next
    }

    public delegate void CurrentPageEventHandler(object sender, object rootElement, PageInfo oldPage, PageInfo newPage, PageChangingVectorEnum changingVector);

    public interface INavigationService
    {
        PageInfo CurrentPage { get; }

        public IReadOnlyDictionary<string, PageInfo> ActivePages { get; }

        event CurrentPageEventHandler CurrentPageChanged;

        event CollectionEventHandler<PageInfo> ActivePagesChanged;

        void PrerenderPage(object page, string pageName = null, string title = null, object backPage = null, object nextPage = null);

        void GoTo(string pageName, object backPage = null, object nextPage = null);

        //Task GoBack();
    }
}
