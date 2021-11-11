namespace BoardModels.AbstractBoardNotifications.Interfaces
{
    public enum RepositoryStateEnum
    {
        Active,
        Stoped
    }
    public delegate void ActionBoardStateHandler(object sender, RepositoryStateEnum state);
    public interface IBoardStateNotification
    {
        RepositoryStateEnum CurrentState { get; }
        event ActionBoardStateHandler BoardStateChanged;
    }
}
