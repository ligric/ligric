using BoardsCore.CommonTypes.Entities.Board;
using Common.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardsCore.Abstractions.BoardsAbstractions.Interfaces
{
    public interface IBoardsService
    {
        event EventHandler<NotifyDictionaryChangedEventArgs<byte, BoardDto>> BoardsChanged;

        IReadOnlyDictionary<byte, BoardDto> Boards { get; }

        Task AddBoard(IEnumerable<BoardEntityConteinerDto> Entities = null);

        Task RemoveBoard(byte key);
    }
}
