using BitZlatoApi.Interfaces;
using BoardRepositories.Abstractions;
using BoardRepositories.BitZlato.Types;
using Common.Interfaces;
using Common.DtoTypes;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using BoardRepositories.Interfaces;

namespace BoardRepositories.BitZlato
{
    public partial class BitZlatoWithTimerRepository : AbstractBoardRepositoryWithTimer<AdDto>
    {
        private static int actionNumber = 0, tryAgain = 0;

        private readonly IBitZlatoRequests bitZlatoApi;

        private readonly Dictionary<string, string> parametrs;

        private readonly Dictionary<long, AdDto> ads = new Dictionary<long, AdDto>();


        public override void Initialize(StateEnum defaultState)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetState(defaultState);
            initializeRates.EndInit();
        }

        public override void Initialize(IDictionary<string, string> filters, StateEnum defaultState)
        {
            ISupportInitializeBoardRepository initializeRates = this;

            initializeRates.BeginInit();
            SetState(defaultState);
            SetParameters(filters);
            initializeRates.EndInit();
        }

        protected override void RenderAds(object sender = null, ElapsedEventArgs e = null)
        {
            timer.Stop();
            if (CurrentState != StateEnum.Active)
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
                            new RateDto(
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
