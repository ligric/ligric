using BoardsCommon;
using Common.EventArgs;
using System;

namespace BoardsCore.Board
{
    public partial class BoardRepository
    {
        public event EventHandler<NotifyDictionaryChangedEventArgs<long, BoardEntityConteinerDto>> EntitiesChanged;
    }
}
