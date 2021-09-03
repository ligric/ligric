using BitZlatoApi;
using BitZlatoApi.Interfaces;
using BoardRepository.BitZlato.Types;
using Common.DtoTypes;
using Common.Enums;
using Common.EventArgs;
using System.Collections;
using System.Timers;

namespace BoardRepository.BitZlato
{
    public partial class BitZlatoWithTimerRepository : AbstractBoardRepositoryWithTimer<AdDto>
    {
        private static int actionNumber = 0, tryAgain = 0;

        private readonly IBitZlatoRequests bitZlatoApi;

        private readonly Dictionary<string, string> parametrs;

        private readonly Dictionary<long, AdDto> ads = new Dictionary<long, AdDto>();

        private event EventHandler<NotifyEnumerableChangedEventArgs<AdDto>> privateStudentsChanged;

        public override event EventHandler<NotifyEnumerableChangedEventArgs<AdDto>> AdsChanged
        {
            add
            {
                lock (((ICollection)ads).SyncRoot)
                {
                    value?.Invoke(this, NotifyActionEnumerableChangedEventArgs.Reset(ads.Values.ToArray(), actionNumber++, DateTimeOffset.Now.ToUnixTimeSeconds()));
                    privateStudentsChanged += value;
                }
            }
            remove
            {
                lock (((ICollection)ads).SyncRoot)
                {
                    privateStudentsChanged -= value;
                }
            }
        }

        protected override void RenderAds(object sender = null, ElapsedEventArgs e = null)
        {
            timer.Stop();
            if (CurrentRepositoryState != RepositoryStateEnum.Active)
                return;

            try
            {
                var result = GetAds();
                if (result != null)
                {
                    CommonAdsDictionaryHandler(result.Result);
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                //TODO : Exception

                if (tryAgain < 5)
                {
                    ++tryAgain;
                    Task.Delay(100);
                    timer.Start();
                    // TODO : нельзя использовать throw new ArgumentException в "void"
                    throw new ArgumentException($"Ой, мы упали, девочки :(\nПопытка запуститься ещё раз: {tryAgain}/5\nMessage: {ex.Message}\nClass: {nameof(BitZlatoWithTimerRepository)}\nMethod: {nameof(RenderAds)}");
                }
                else
                {
                    // TODO : нельзя использовать throw new ArgumentException в "void"
                    throw new ArgumentException($"Ой, мы упали, девочки :(\nПопытка запуститься ещё раз: а всё, попыток больше нет. Конечная -- спускайтесь.\nMessage: {ex.Message}\nClass: {nameof(BitZlatoWithTimerRepository)}\nMethod: {nameof(RenderAds)}");
                }
            }
        }

        private async Task<IEnumerable<AdDto>> GetAds()
        {
            var responseAds = await bitZlatoApi.GetAds(parametrs);

            var bitZlatoEnumerable = responseAds.Data.Select(
                adApi => new AdDto(adApi.Id,
                            new TraderDto(
                                adApi.Owner,
                                adApi.ownerBalance,
                                adApi.OwnerLastActivity,
                                adApi.IsOwnerVerificated,
                                adApi.OwnerTrusted),
                            new PaymethodDto(
                                adApi.Paymethod.Id,
                                adApi.Paymethod.Name),
                            new Types.RateDto(
                                new CurrencyDto(
                                    adApi.Currency,
                                    null,
                                    CurrencyTypeEnum.Bank),
                                new CurrencyDto(
                                    adApi.Cryptocurrency,
                                    null,
                                    CurrencyTypeEnum.Crypto),
                                adApi.Rate),
                            new LimitDto(
                                adApi.LimitCurrency.Min,
                                adApi.LimitCurrency.Max,
                                adApi.LimitCurrency.RealMax),
                            new LimitDto(
                                adApi.LimitCryptocurrency.Min,
                                adApi.LimitCryptocurrency.Max,
                                adApi.LimitCryptocurrency.RealMax),
                            adApi.Type == "selling" ? AdTypeEnum.Selling : AdTypeEnum.Buying, adApi.SafeMode));

            return bitZlatoEnumerable;
        }

    }
}
