using Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using BoardRepositories.Interfaces;
using Common.Enums;

namespace BoardRepositories.Abstractions
{
    public abstract partial class AbstractBoardRepository<T> : IBoardRepository<T>, IStateNotification, IBoardParamsNotification, ISupportInitializeBoardRepository
    {
        #region Properties
        public IDictionary<string, string> Parameters { get; private set; } = new Dictionary<string, string>();
        public StateEnum CurrentState { get; protected set; }
        #endregion

        #region Notifications
        public virtual bool SetState(StateEnum state)
        {
            StateEnum old = CurrentState;
            if (state == old)
                return false;

            CurrentState = state;
            RaiseActionState(state);
            return true;
        }

        public virtual bool SetParameters(IDictionary<string, string> parameters)
        {
            if (parameters == null)
                return false;

            Dictionary<string, string> old = Parameters.ToDictionary(entry => entry.Key, entry => entry.Value);
            Dictionary<string, string> @new = parameters.ToDictionary(entry => entry.Key, entry => entry.Value);

            if (@new == old)
                return false;

            Parameters = @new;
            RaiseActionParameters(@new);
            return true;
        }
        #endregion

        #region ISupportInitialize
        public bool IsBeginInit { get; protected set; }
        public abstract void BeginInit();

        public abstract void EndInit();

        public abstract void Initialize(StateEnum state);

        public abstract void Initialize(IDictionary<string, string> filters, StateEnum state);
        #endregion

        #region Constructors
        public AbstractBoardRepository(StateEnum state = StateEnum.Stoped)
        {
            if (IsBeginInit)
                ((ISupportInitializeBoardRepository)this).Initialize(state);
        }

        public AbstractBoardRepository(IDictionary<string, string> parameters, StateEnum state = StateEnum.Stoped)
        {
            if (IsBeginInit)
                ((ISupportInitializeBoardRepository)this).Initialize(parameters, state);
        }
        #endregion
    }
}
