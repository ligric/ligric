using BoardsCore.CommonTypes.Entities.Board;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BoardsCore.Board
{
    public partial class BoardService
    {
        public byte Id { get; }

        private readonly Dictionary<long, BoardEntityConteinerDto> entities = new Dictionary<long, BoardEntityConteinerDto>();
        public ReadOnlyDictionary<long, BoardEntityConteinerDto> Entities { get; }

        public BoardService(byte id)
        {
            Id = id;
            Entities = new ReadOnlyDictionary<long, BoardEntityConteinerDto>(entities);
        }
    }
}
