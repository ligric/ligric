using BoardsCore.Abstractions.BoardsAbstractions.Interfaces;
using BoardsCore.Board;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BoardsCore.Boards
{
    public sealed partial class BoardsService : IBoardsService
    {
        private readonly Dictionary<byte, BoardService> boards = new Dictionary<byte, BoardService>();
        public IReadOnlyDictionary<byte, BoardService> Boards { get; } = new Dictionary<byte, BoardService>();

        public BoardsService()
        {
            Boards = new ReadOnlyDictionary<byte, BoardService>(boards);
        }
    }
}
