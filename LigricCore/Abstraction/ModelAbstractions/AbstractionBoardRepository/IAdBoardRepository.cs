namespace AbstractionBoardRepository
{  
    public interface IAdBoardRepository : IAdBoardNameNotification, IAdBoardDictionaryNotification, IAdBoardRepositoryStateNotification
    {
        void SetFilters(IDictionary<string, string> newFilters);
        void SetName(string name);
        void SetAdBoardState(RepositoryStateEnum state);
    }
}
