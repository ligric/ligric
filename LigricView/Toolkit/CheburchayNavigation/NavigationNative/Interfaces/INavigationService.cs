namespace CheburchayNavigation.Native.Interfaces
{
    public interface INavigationService : IPagesService, IPinsService
    {
        string RootKey { get; }
    }
}
