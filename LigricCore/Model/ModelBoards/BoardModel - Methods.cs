using AbstractionBoardRepository;

namespace BoardRepository
{
    public partial class BoardBitZlatoRepository : IAdBoardRepository
    {
        public void SetFilters(IDictionary<string, string> newFilters)
        {
            timer.Stop();
            SetFiltersAndSendAction(newFilters);
            RenderAds();
        }

        public void SetName(string name)
        {
            timer.Stop();
            SetNameAndSendAction(name);
            RenderAds();
        }

        public void SetAdBoardState(RepositoryStateEnum state)
        {
            timer.Stop();
            SetAdBoardStateAndSendAction(state);
            RenderAds();
        }

        private void RenderAds()
        {
            timer.Stop();
            if(CurrentRepositoryState != RepositoryStateEnum.Active)
                return;

            timer.Start();
        }
    }
}
