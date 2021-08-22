using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractionBoardRepository
{
    public enum RepositoryStateEnum
    {
        Active,
        Stoped
    }
    public delegate void ActionRepositoryStateHandler(object sender, RepositoryStateEnum state);
    public interface IAdBoardRepositoryStateNotification
    {
        RepositoryStateEnum CurrentRepositoryState { get; }
        event ActionRepositoryStateHandler RepositoryStateChanged;
    }
}
