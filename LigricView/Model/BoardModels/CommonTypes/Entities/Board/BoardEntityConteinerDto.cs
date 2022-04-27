using BoardsShared.CommonTypes.Enums;

namespace BoardsShared.CommonTypes.Entities.Board
{
    public class BoardEntityConteinerDto
    {
        public BoardEntityType Type { get; }
        public object Entity { get; }

        public BoardEntityConteinerDto(BoardEntityType type, object entity)
        {
            Type = type; Entity = entity;
        }
    }
}
