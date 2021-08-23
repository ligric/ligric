using AbstractionBitZlatoRequests;
using AbstractionBoardRepository;
using AbstractionBoardRepository.Abstracts;
using AbstractionBoardRepository.Interfaces;

namespace BoardRepository
{
    public partial class BoardBitZlatoRepository : AbstractAdBoardWithTimerNotifications, IAdBoardRepositoryWIthTimer
    {
        private IBitZlatoRequestsService bitZlatoRequests;

        public bool SetUpdateTime(TimeSpan time)
        {
            Timer.Stop();
            if (time.Milliseconds < 250)
                return false;

            if (SetUpdateTimeAndSendAction(time))
            {
                RenderAds();
                return true;
            }

            return false;
        }

        public bool SetFilters(IDictionary<string, string> newFilters)
        {
            Timer.Stop();
            if (SetFiltersAndSendAction(newFilters))
            {
                RenderAds();
                return true;
            }

            return false;
        }

        public bool SetName(string name)
        {
            Timer.Stop();
            if (SetNameAndSendAction(name))
            {
                RenderAds();
                return true;
            }
            
            return false;
        }

        public bool SetAdBoardState(RepositoryStateEnum state)
        {
            Timer.Stop();
            if (SetAdBoardStateAndSendAction(state))
            {
                RenderAds();
                return true;
            }

            return false;
        }

        protected override void RenderAds(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            Timer.Stop();
            if (CurrentRepositoryState != RepositoryStateEnum.Active)
                return;

            try
            {
                var result = Task.Run(async () => await bitZlatoRequests.GetAdsFromFilters(Filters));
                if (result.Result.Data != null)
                {

                }
            }
            catch (Exception)
            {
                
            }
           

            Timer.Start();
        }
    }
}
