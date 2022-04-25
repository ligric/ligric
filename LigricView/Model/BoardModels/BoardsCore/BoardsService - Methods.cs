#if NETSTANDARD
using Common.Extensions;
#endif
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardsShared.BoardsCore
{
    public sealed partial class BoardsService
    {
        public async Task AddNewBoard(string key, string value)
        {
            if (!boards.TryAdd(key, value))
                throw new ArgumentException($"Unable to add {key} key to dictionary.");


        }
    }
}
