using Common.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardsShared.Abstractions.BoardsAbstractions.Interfaces
{
    public interface IBoardsService
    {
        event EventHandler<NotifyDictionaryChangedEventArgs<string, string>> BoardsChanged;

        IReadOnlyDictionary<string, string> Boards { get; }

        Task AddBoard(string key, string value);

        Task RemoveBoard(string key, string value);
    }
}
