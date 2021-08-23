using AbstractionBitZlatoRequests;
using System.ComponentModel;

namespace AbstractionBoardRepository.Interfaces
{
    public interface ISupportInitializeBoardRepository : ISupportInitialize
    {
        bool IsBeginInit { get; }

        void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState);
        void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, string name);
        void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, IBitZlatoRequestsService bitZlatoRequests);
        void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, string name, IBitZlatoRequestsService bitZlatoRequests);

        
    }
}
