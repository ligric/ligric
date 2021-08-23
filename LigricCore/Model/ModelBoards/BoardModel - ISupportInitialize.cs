using AbstractionBoardRepository;
using System.ComponentModel;

namespace BoardRepository
{
    public partial class BoardBitZlatoRepository : ISupportInitializeBoardRepository, IAdBoardFiltersNotification
    {
        private readonly System.Timers.Timer timer = new System.Timers.Timer();

        void ISupportInitialize.BeginInit()
        {
            if (IsBeginInit)
                throw new MethodAccessException("Нельзя начинать новую инициализацию пока не закончена предыдущая транзакциия инициализации.");
            IsBeginInit = true;
            timer.Stop();
        }

        void ISupportInitialize.EndInit()
        {
            if (!IsBeginInit)
                throw new MethodAccessException("Транзакциия инициализации не была начата.");

            IsBeginInit = false;
        }

        void ISupportInitializeBoardRepository.Initialize(IDictionary<string, string> filters)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            //SetNewFilters(filters);
            initializeRates.EndInit();
        }

        public bool IsBeginInit { get; private set; }
    }
}
