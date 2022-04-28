using BoardsCore.CommonTypes.Entities.Board;
using Common.EventArgs;
using System;

namespace BoardsCore
{
    public sealed partial class BoardsService
    {
        public event EventHandler<NotifyDictionaryChangedEventArgs<byte, BoardDto>> BoardsChanged;
    }
}
