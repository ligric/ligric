using AbstractionBoardRepository;
using System.Collections.Generic;

namespace BoardRepository.BitZlato
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

        private void RenderAds()
        {
            timer.Stop();
            if(CurrentRepositoryState != RepositoryStateEnum.Active)
                return;


            timer.Start();
        }
    }
}
