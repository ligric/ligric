using BoardsCore.CommonTypes.Entities.Board;
using Common.EventArgs;
using System;

namespace BoardsCore.Board
{
    public partial class BoardService
    {
        public event EventHandler<NotifyDictionaryChangedEventArgs<long, BoardEntityConteinerDto>> EntitiesChanged;
    }
}
