using BoardsShared.Abstractions.BoardsAbstractions.Interfaces;
using BoardsShared.CommonTypes.Entities.Board;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BoardsShared
{
    public sealed partial class BoardsService : IBoardsService
    {
        private readonly Dictionary<byte, BoardDto> boards = new Dictionary<byte, BoardDto>();
        public IReadOnlyDictionary<byte, BoardDto> Boards { get; } = new Dictionary<byte, BoardDto>();

        public BoardsService()
        {
            Boards = new ReadOnlyDictionary<byte, BoardDto>(boards);
        }
    }
}
