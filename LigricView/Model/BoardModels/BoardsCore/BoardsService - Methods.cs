using BoardsShared.CommonTypes.Entities.Board;
using Common.EventArgs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardsShared.BoardsCore
{
    public sealed partial class BoardsService
    {
        public Task AddBoard(IEnumerable<BoardEntityConteinerDto> entities = null)
        {
            var newKey = GetFreeKey();
            var newBoard = new BoardDto(newKey, entities);
            boards.Add(newKey, newBoard);

            BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair<byte, BoardDto>(newKey, newBoard, 0, 0));

            return Task.CompletedTask;
        }

        public Task RemoveBoard(byte key)
        {
            if (!boards.Remove(key))
                throw new ArgumentException($"Unable to куьщму {key} key from dictionary.");

            BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<byte, BoardDto>(key, 0, 0));

            return Task.CompletedTask;
        }

        private byte GetFreeKey()
        {
            byte lastKey = default(byte);

            lock (((ICollection)boards).SyncRoot)
            {
                lastKey = (byte)(boards.Count > 0 ? boards.Keys.Last() + 1 : 0);

                if (lastKey >= 255)
                    throw new StackOverflowException($"Last boards key is {lastKey}");
            }
            
            return lastKey;
        }
    }
}
