using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BoardsShared.BoardsCore
{
    public sealed partial class BoardsService
    {
        private readonly Dictionary<string, string> boards = new Dictionary<string, string>();
        public IReadOnlyDictionary<string, string> Boards { get; } = new Dictionary<string, string>();

        public BoardsService()
        {
            Boards = new ReadOnlyDictionary<string, string>(boards);
        }
    }
}
