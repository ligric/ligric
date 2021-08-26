using AbstractionBitZlatoRequests;
using AbstractionBoardRepository;
using AbstractionBoardRepository.Abstracts;
using AbstractionBoardRepository.Interfaces;

namespace BoardRepository
{
    public partial class BoardBitZlatoRepository : AbstractAdBoardWithTimerNotifications
    {
        public BoardBitZlatoRepository() 
            : base()
        { }

        public BoardBitZlatoRepository(IDictionary<string, string> filters, TimeSpan updateTime, RepositoryStateEnum repositoryState)
            : base(filters, updateTime, repositoryState)
        {
            //((ISupportInitializeBoardRepository)this).Initialize(filters, repositoryState);
        }

        public BoardBitZlatoRepository(IDictionary<string, string> filters, TimeSpan updateTime, RepositoryStateEnum repositoryState, string boardName)
            : base(filters, updateTime, repositoryState)
        {
            //((ISupportInitializeBoardRepository)this).Initialize(filters, repositoryState, boardName);
        }

        public BoardBitZlatoRepository(IDictionary<string, string> filters, TimeSpan updateTime, RepositoryStateEnum repositoryState, IBitZlatoRequestsService bitZlatoRequests)
            : base(filters, updateTime, repositoryState)
        {
            //((ISupportInitializeBoardRepository)this).Initialize(filters, repositoryState, bitZlatoRequests);
        }

        public BoardBitZlatoRepository(IDictionary<string, string> filters, TimeSpan updateTime, RepositoryStateEnum repositoryState, string boardName, IBitZlatoRequestsService bitZlatoRequests)
            : base(filters, updateTime, repositoryState, bitZlatoRequests)
        {
            //((ISupportInitializeBoardRepository)this).Initialize(filters, repositoryState, boardName, bitZlatoRequests);
        }
    }
}
