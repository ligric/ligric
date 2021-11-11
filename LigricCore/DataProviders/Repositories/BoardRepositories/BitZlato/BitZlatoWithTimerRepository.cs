using BitZlatoApi.Interfaces;
using BoardRepositories.Abstractions;
using BoardRepositories.BitZlato.Types;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using BoardRepositories.Interfaces;
using BitZlatoApi.Types;
using BoardRepositories.Types;
using BoardRepositories.Enums;

namespace BoardRepositories.BitZlato
{
    public partial class BitZlatoWithTimerRepository : AbstractBoardRepositoryWithTimer<Ad>
    {
        private static int actionNumber = 0, tryAgain = 0;

        private readonly IBitZlatoRequests bitZlatoApi;

        private readonly Dictionary<string, string> parametrs;

        private readonly Dictionary<long, Ad> ads = new Dictionary<long, Ad>();


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
                var result = Task.Run(async() => await GetAdsAsync());
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

        private async Task<IEnumerable<Ad>> GetAdsAsync()
        {
            var responseAds = await bitZlatoApi.GetJsonAdsAsync(parametrs);

            var bitZlatoEnumerable = responseAds.Data.Select(
                adApi => new Ad(adApi.Id,
                            new Trader(
                                adApi.Owner,
                                adApi.ownerBalance,
                                adApi.OwnerLastActivity,
                                adApi.IsOwnerVerificated,
                                adApi.OwnerTrusted),
                            new Paymethod(
                                adApi.Paymethod.Id,
                                adApi.Paymethod.Name),
                            new Rate(
                                new Currency(
                                    adApi.Currency,
                                    null,
                                    CurrencyTypeEnum.Bank),
                                new Currency(
                                    adApi.Cryptocurrency,
                                    null,
                                    CurrencyTypeEnum.Crypto),
                                adApi.Rate),
                            new Limit(
                                adApi.LimitCurrency.Min,
                                adApi.LimitCurrency.Max,
                                adApi.LimitCurrency.RealMax),
                            new Limit(
                                adApi.LimitCryptocurrency.Min,
                                adApi.LimitCryptocurrency.Max,
                                adApi.LimitCryptocurrency.RealMax),
                            adApi.Type == "selling" ? AdTypeEnum.Selling : AdTypeEnum.Buying, adApi.SafeMode));

            return bitZlatoEnumerable;
        }

    }
}
