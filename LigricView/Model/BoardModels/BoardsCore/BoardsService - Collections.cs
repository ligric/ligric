using BoardsShared.Abstractions.BoardsAbstractions.Interfaces;
using BoardsShared.CommonTypes.Entities.Board;
using Common.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BoardsShared.BoardsCore
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
