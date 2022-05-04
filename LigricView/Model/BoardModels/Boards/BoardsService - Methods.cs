using BoardsCore.Board;
using Common.EventArgs;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace BoardsCore.Boards
{
    public sealed partial class BoardsService
    {
        public BoardService CurrentBoard { get; private set; }

        public Task AddBoard()
        {
            var newKey = GetFreeKey();
            var newBoard = new BoardService(newKey);
            boards.Add(newKey, newBoard);

            BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair<byte, BoardService>(newKey, newBoard, 0, 0));

            return Task.CompletedTask;
        }

        public Task RemoveBoard(byte key)
        {
            if (!boards.Remove(key))
                throw new ArgumentException($"Unable to куьщму {key} key from dictionary.");

            BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<byte, BoardService>(key, 0, 0));

            return Task.CompletedTask;
        }

        public Task SetNewCurrentBoard(byte key)
        {
            if (!boards.TryGetValue(key, out BoardService newBoardService))
                throw new ArgumentNullException($"Board with key {key} is not found.");

            var oldBoard = CurrentBoard;
            CurrentBoard = newBoardService;

            CurrentBoardChanged?.Invoke(this, oldBoard, newBoardService);

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
