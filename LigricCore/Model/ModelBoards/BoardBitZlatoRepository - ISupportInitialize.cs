using AbstractionBitZlatoRequests;
using AbstractionBoardRepository;
using AbstractionBoardRepository.Interfaces;
using System.ComponentModel;

namespace BoardRepository
{
    public partial class BoardBitZlatoRepository : ISupportInitializeBoardRepository
    {
        public bool IsBeginInit { get; private set; }

        void ISupportInitialize.BeginInit()
        {
            if (IsBeginInit)
                throw new MethodAccessException("Нельзя начинать новую инициализацию пока не закончена предыдущая транзакциия инициализации.");

            IsBeginInit = true;
            Timer.Stop();
        }

        void ISupportInitialize.EndInit()
        {
            if (!IsBeginInit)
                throw new MethodAccessException("Транзакциия инициализации не была начата.");

            IsBeginInit = false;
            RenderAds();
        }

        void ISupportInitializeBoardRepository.Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetFiltersAndSendAction(filters);
            SetAdBoardStateAndSendAction(repositoryState);
            initializeRates.EndInit();
        }

        void ISupportInitializeBoardRepository.Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, string name)
        {
            ISupportInitializeBoardRepository initializeRates = this;
               
            initializeRates.BeginInit();
            SetFiltersAndSendAction(filters);
            SetAdBoardStateAndSendAction(repositoryState);
            SetNameAndSendAction(name);
            initializeRates.EndInit();
        }

        public void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, IBitZlatoRequestsService bitZlatoRequests)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetFiltersAndSendAction(filters);
            SetAdBoardStateAndSendAction(repositoryState);
            this.bitZlatoRequests = bitZlatoRequests;
            initializeRates.EndInit();
        }

        public void Initialize(IDictionary<string, string> filters, RepositoryStateEnum repositoryState, string name, IBitZlatoRequestsService bitZlatoRequests)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetFiltersAndSendAction(filters);
            SetAdBoardStateAndSendAction(repositoryState);
            SetNameAndSendAction(name);
            this.bitZlatoRequests = bitZlatoRequests;
            initializeRates.EndInit();
        }
    }
}
