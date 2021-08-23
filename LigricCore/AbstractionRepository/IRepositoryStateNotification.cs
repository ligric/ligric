namespace AbstractionBoardRepository
{
    public enum RepositoryStateEnum
    {
        Active,
        Stoped
    }
    public delegate void ActionRepositoryStateHandler(object sender, RepositoryStateEnum state);
    public interface IRepositoryStateNotification
    {
        RepositoryStateEnum CurrentRepositoryState { get; }
        event ActionRepositoryStateHandler RepositoryStateChanged;
    }
}
