using Common.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace BoardRepositories.Interfaces
{
    public interface ISupportInitializeBoardRepository : ISupportInitialize
    {
        bool IsBeginInit { get; }

        void Initialize(StateEnum defaultState);

        void Initialize(IDictionary<string, string> filters, StateEnum defaultState);
    }
}
