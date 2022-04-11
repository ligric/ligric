using Common.Delegates;
using Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigricMvvmToolkit.Navigation
{
    public enum PageChangingVectorEnum
    {
        Back,
        Next,
        New
    }

    public enum PageActiveAction
    {
        Prerender,
        GoWithPrerender,
        Destroyed
    }


    public delegate void CurrentPageEventHandler(object sender, object rootElement, PageInfo oldPage, PageInfo newPage, PageChangingVectorEnum changingVector, int? index);
    public delegate void PagePrerenderEventHandler<T>(object sender, object rootElement, PageActiveAction action, T item);


    public interface INavigationService
    {
        PageInfo CurrentPage { get; }

        public IReadOnlyDictionary<string, PageInfo> ActivePages { get; }

        event CurrentPageEventHandler CurrentPageChanged;

        event PagePrerenderEventHandler<PageInfo> ActivePagesChanged;

        void PrerenderPage(object page, string pageName = null, object vm = null, string title = null, object backPage = null, object nextPage = null);

        void GoTo(string pageName, object backPage = null, object nextPage = null);

        //Task GoBack();
    }
}
