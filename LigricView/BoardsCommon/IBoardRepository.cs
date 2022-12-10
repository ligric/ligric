using BoardsCommon.Enums;
using Common.EventArgs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BoardsCommon
{
    public interface IBoardRepository
    {
        long Id { get; }

        string Name { get; }

        ReadOnlyDictionary<long, BoardEntityConteinerDto> Entities { get; }

        event EventHandler<NotifyDictionaryChangedEventArgs<long, BoardEntityConteinerDto>> EntitiesChanged;

        Task AddEntity(BoardEntityType type);
    }
}
