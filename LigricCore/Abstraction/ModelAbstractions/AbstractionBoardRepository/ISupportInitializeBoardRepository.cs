using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AbstractionBoardRepository
{
    public interface ISupportInitializeBoardRepository : ISupportInitialize, IAdBoardFiltersNotification
    {
        bool IsBeginInit { get; }

        void Initialize(IDictionary<string, string> filters);
    }
}
