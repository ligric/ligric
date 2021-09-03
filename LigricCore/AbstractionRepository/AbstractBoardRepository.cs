using Common.EventArgs;

namespace BoardRepository
{
    public abstract class AbstractBoardRepository<T>: IBoardRepository<T>, IRepositoryStateNotification
    {
        public abstract event EventHandler<NotifyEnumerableChangedEventArgs<T>> AdsChanged;
        public RepositoryStateEnum CurrentRepositoryState { get; private set; }
        public event ActionRepositoryStateHandler RepositoryStateChanged;

        public virtual bool SetAdBoardStateAndSendAction(RepositoryStateEnum repositoryState)
        {
            if (repositoryState == CurrentRepositoryState)
                return false;

            CurrentRepositoryState = repositoryState;
            RepositoryStateChanged?.Invoke(this, repositoryState);
            return true;
        }
        /*------------------------------------------------------------------------------------------------------------------*/
        #region Constructors
        protected AbstractBoardRepository(RepositoryStateEnum repositoryState = RepositoryStateEnum.Stoped) => CurrentRepositoryState = repositoryState;
        #endregion
    }
}
