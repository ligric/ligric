using System.Threading.Tasks;

namespace LigricMvvm.Navigation
{
    public enum PageChangingVectorEnum
    {
        Back,
        Next
    }

    public delegate void PageChangeHandler(object sender, object oldPage, object newPage, PageChangingVectorEnum changingVector);


    public interface INavigationService
    {

        object CurrentPage { get; }

        event PageChangeHandler PageChanged;

        Task PrerenderPage(object page, string pageName, object backPage = null, object nextPage = null);

        Task GoTo(string pageName);

        //Task NavigateTo(Type page);

        //Task NavigateTo(Type page, byte number);

        //Task NavigateTo(string page, object parameter);

        //Task GoBack();
    }
}
