using BoardsCore.Board;
using Common.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardsCore.Abstractions.BoardsAbstractions.Interfaces
{
    public interface IBoardsService
    {
        event EventHandler<NotifyDictionaryChangedEventArgs<byte, BoardService>> BoardsChanged;

        IReadOnlyDictionary<byte, BoardService> Boards { get; }

        Task AddBoard();

        Task RemoveBoard(byte key);
    }
}
