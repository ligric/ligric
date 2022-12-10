using System;

namespace BoardsCommon
{
    public class BoardEntityConteinerDto
    {
        public Type Type { get; }

        public object Entity { get; }

        public BoardEntityConteinerDto(Type type, object entity)
        {
            Type = type; Entity = entity;
        }
    }
}
