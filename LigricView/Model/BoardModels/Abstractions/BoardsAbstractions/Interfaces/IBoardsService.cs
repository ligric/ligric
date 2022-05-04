using BoardsCore.Board;
using BoardsCore.Notifications.Delegates;
using Common.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardsCore.Abstractions.BoardsAbstractions.Interfaces
{
    public interface IBoardsService
    {
        BoardService CurrentBoard { get; }

        event ElementChangedHandler<BoardService> CurrentBoardChanged;
        event EventHandler<NotifyDictionaryChangedEventArgs<byte, BoardService>> BoardsChanged;

        IReadOnlyDictionary<byte, BoardService> Boards { get; }

        Task AddBoard();

        Task RemoveBoard(byte key);

        Task SetNewCurrentBoard(byte key);
    }
}
