using BoardsCore.Board;
using Common.EventArgs;
using System;

namespace BoardsCore.Boards
{
    public sealed partial class BoardsService
    {
        public event EventHandler<NotifyDictionaryChangedEventArgs<byte, BoardService>> BoardsChanged;
    }
}
