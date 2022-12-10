using System;
using System.Threading.Tasks;

namespace BoardsCore.Boards
{
    //public sealed partial class BoardsRespository
    //{
    //    public IBoardRepository CurrentBoard { get; private set; }

    //    public Task AddBoard(string name, string publicKey, string privateKey)
    //    {


    //        boards.Add(mewRepository.Id, mewRepository);

    //        BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(mewRepository.Id, mewRepository, 0, 0));

    //        return Task.CompletedTask;
    //    }

    //    public Task RemoveBoard(long key)
    //    {
    //        if (!boards.Remove(key))
    //            throw new ArgumentException($"Unable to куьщму {key} key from dictionary.");

    //        BoardsChanged?.Invoke(this, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<long, IBoardRepository>(key, 0, 0));

    //        return Task.CompletedTask;
    //    }

    //    public Task SetNewCurrentBoard(IBoardRepository repository)
    //    {
    //        if (!boards.TryGetValue(repository.Id, out IBoardRepository newBoardService))
    //            throw new ArgumentNullException($"Board with key {repository.Id} is not found.");

    //        var oldBoard = CurrentBoard;
    //        CurrentBoard = newBoardService;

    //        CurrentBoardChanged?.Invoke(this, oldBoard, newBoardService);

    //        return Task.CompletedTask;
    //    }
    //}
}
