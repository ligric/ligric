using BoardsCore.Board;
using BoardsCore.Notifications.Delegates;
using Common.EventArgs;
using System;

namespace BoardsCore.Boards
{
    public sealed partial class BoardsService
    {
        public event EventHandler<NotifyDictionaryChangedEventArgs<byte, BoardService>> BoardsChanged;
        public event ElementChangedHandler<BoardService> CurrentBoardChanged;
    }
}
