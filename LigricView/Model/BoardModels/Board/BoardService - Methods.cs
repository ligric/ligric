using BoardsCommon.Enums;
using BoardsCore.CommonTypes.Entities;
using BoardsCore.CommonTypes.Entities.Board;
using Common.EventArgs;
using System;
using System.Threading.Tasks;

namespace BoardsCore.Board
{
    public partial class BoardService
    {
        public Task AddEntity(BoardEntityType type)
        {
            object entity = null;
            int id = new Random().Next();
            if (type == BoardEntityType.Ad)
            {
                entity = new AdDto(id);
            }

            var newEntity = new BoardEntityConteinerDto(type, entity);
            entities.Add(id, newEntity);

            EntitiesChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair<long, BoardEntityConteinerDto>(id, newEntity, 0, 0));

            return Task.CompletedTask;
        }

        //public Task RemoveBoard(byte key)
        //{
        //    if (!boards.Remove(key))
        //        throw new ArgumentException($"Unable to куьщму {key} key from dictionary.");

        //    BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<byte, BoardService>(key, 0, 0));

        //    return Task.CompletedTask;
        //}
    }
}
