using System.Threading.Tasks;

namespace LigricMvvm.Navigation
{
    public enum PageChangingVectorEnum
    {
        Back,
        Next
    }

    public delegate void CurrentPageChangeHandler(object sender, object oldPage, object newPage, PageChangingVectorEnum changingVector);


    public interface INavigationService
    {

        object CurrentPage { get; }

        event CurrentPageChangeHandler CurrentPageChanged;

        Task PrerenderPage(object page, string pageName = null, object backPage = null, object nextPage = null);

        Task GoTo(string pageName, object page = null, object backPage = null, object nextPage = null);

        //Task NavigateTo(Type page);

        //Task NavigateTo(Type page, byte number);

        //Task NavigateTo(string page, object parameter);

        //Task GoBack();
    }
}
