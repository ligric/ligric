#if NETSTANDARD
using Common.Extensions;
#endif
using Common.EventArgs;
using System;
using System.Threading.Tasks;

namespace BoardsShared.BoardsCore
{
    public sealed partial class BoardsService
    {
        public async Task AddBoard(string key, string value)
        {
            if (!boards.TryAdd(key, value))
                throw new ArgumentException($"Unable to add {key} key to dictionary.");

            BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair<string, string>(key, value, 0, 0));
        }

        public async Task RemoveBoard(string key, string value)
        {
            if (!boards.Remove(key))
                throw new ArgumentException($"Unable to куьщму {key} key from dictionary.");

            BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<string, string>(key, 0, 0));
        }
    }
}
