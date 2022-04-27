using BoardsShared.CommonTypes.Entities.Board;
using Common.EventArgs;
using System;

namespace BoardsShared.BoardsCore
{
    public sealed partial class BoardsService
    {
        public event EventHandler<NotifyDictionaryChangedEventArgs<byte, BoardDto>> BoardsChanged;
    }
}
