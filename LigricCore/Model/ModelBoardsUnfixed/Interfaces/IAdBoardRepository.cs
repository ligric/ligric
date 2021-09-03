namespace AbstractionBoardRepository.Interfaces
{  
    public interface IAdBoardRepository : IAdBoardNameNotification, IAdBoardDictionaryNotification, IRepositoryStateNotification, IAdBoardFiltersNotification
    {
        bool SetFilters(IDictionary<string, string> newFilters);
        bool SetName(string name);
        bool SetAdBoardState(RepositoryStateEnum state);
    }
}
