using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BoardsCore.CommonTypes.Entities.Board
{
    public class BoardDto
    {
        public byte Id { get; }
        public ReadOnlyCollection<BoardEntityConteinerDto> Entities { get; }
        private static readonly ReadOnlyCollection<BoardEntityConteinerDto> empty = new ReadOnlyCollection<BoardEntityConteinerDto>(new BoardEntityConteinerDto[0]);


        public BoardDto(byte id, IEnumerable<BoardEntityConteinerDto> entities)
        {
            Id = id;
            Entities = entities == null
                                ? empty
                                : new ReadOnlyCollection<BoardEntityConteinerDto>(entities.ToArray());
        }
    }
}
