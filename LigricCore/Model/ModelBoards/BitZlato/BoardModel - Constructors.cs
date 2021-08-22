using AbstractionBoardRepository;

namespace BoardRepository.BitZlato
{
    public partial class BoardBitZlatoRepository : AbstractAdBoardNotifications, IAdBoardDictionaryNotification, IAdBoardNameNotification, IAdBoardFiltersNotification
    {
        public BoardBitZlatoRepository() 
            : base()
        {

            //FiltersChanged += (s,e) => timer.Stop();
        }
    }
}
