using AbstractionBitZlatoRequests;
using AbstractionBitZlatoRequests.DtoTypes;
using AbstractionBoardRepository;
using AbstractionBoardRepository.Abstracts;
using AbstractionBoardRepository.Interfaces;
using Common.DtoTypes.Board;
using Common.Enums;

namespace BoardRepository
{
    public partial class BoardBitZlatoRepository : AbstractAdBoardWithTimerNotifications, IAdBoardRepositoryWIthTimer
    {
        public bool SetUpdateTime(TimeSpan time)
        {
            timer.Stop();
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
            timer.Stop();
            if (SetFiltersAndSendAction(newFilters))
            {
                RenderAds();
                return true;
            }

            return false;
        }

        public bool SetName(string name)
        {
            timer.Stop();
            if (SetNameAndSendAction(name))
            {
                RenderAds();
                return true;
            }
            
            return false;
        }

        public bool SetAdBoardState(RepositoryStateEnum state)
        {
            timer.Stop();
            if (SetAdBoardStateAndSendAction(state))
            {
                RenderAds();
                return true;
            }

            return false;
        }

        protected override void RenderAds(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            timer.Stop();
            if (CurrentRepositoryState != RepositoryStateEnum.Active)
                return;

            try
            {
                var result = Task.Run(async () => await bitZlatoRequests.GetAdsFromFilters(Filters));

                if (result.Result.Data == null)
                    return;

                var list = new List<AdDto>();



                foreach (var newAd in (IEnumerable<Ad>)result.Result.Data)
                {
                    list.Add(new AdDto(newAd.Id,
                             new TraderDto(newAd.Owner, newAd.ownerBalance, newAd.OwnerLastActivity, newAd.IsOwnerVerificated, newAd.OwnerTrusted),
                             new PaymethodDto(newAd.Paymethod.Id, newAd.Paymethod.Name),
                             new RateDto(new CurrencyDto(newAd.Currency, null, CurrencyTypeEnum.Bank),
                                         new CurrencyDto(newAd.Cryptocurrency, null, CurrencyTypeEnum.Crypto),
                                         newAd.Rate),
                             new LimitDto(newAd.LimitCurrency.Min, newAd.LimitCurrency.Max, newAd.LimitCurrency.RealMax),
                             new LimitDto(newAd.LimitCryptocurrency.Min, newAd.LimitCryptocurrency.Max, newAd.LimitCryptocurrency.RealMax),
                                          newAd.Type == "selling" ? AdTypeEnum.Selling : AdTypeEnum.Buying,
                             newAd.SafeMode));
                }

                _ = NewAdsHandler(list);
            }
            catch (Exception ex)
            {
                var exception = ex;
            }

            timer.Start();
        }

        public bool StartRepository()
        {
            throw new NotImplementedException();
        }

        public bool StopRepository()
        {
            throw new NotImplementedException();
        }
    }
}
