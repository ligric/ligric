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
using BoardRepositories.Types;
using BoardRepositories.Enums;
using BitZlatoApi.Types;
using Common.Extensions;

namespace BoardRepositories.BitZlato
{
    public partial class BitZlatoWithTimerRepository : AbstractBoardRepositoryWithTimer<long, Ad>
    {
        private int tryAgain = 0;

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

        protected async override void RenderAds(object sender = null, ElapsedEventArgs e = null)
        {
            timer.Stop();
            if (CurrentState != StateEnum.Active)
                return;

            try
            {
                var result = await GetAdsAsync();
                if (result != null)
                {
                    ads.NewElementsHandler(this, result, privateAdsChanged, ref actionNumber);
                    timer.Start();
                }
                else
                {
                    throw new ArgumentException($"Error lol kek azazek test");
                }
            }
            catch (Exception ex)
            {
                //TODO : Exception

                if (tryAgain < 5)
                {
                    ++tryAgain;
                    await Task.Delay(100);
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

        private async Task<IDictionary<long, Ad>> GetAdsAsync()
        {
            var responseAds = await bitZlatoApi.GetJsonAdsAsync(parametrs);

            Dictionary<long, Ad> adsDict = new Dictionary<long, Ad>();
            foreach (var item in responseAds.Data)
                adsDict.Add(item.Id, item.ToAd());

            return adsDict;
        }
    }

    internal static class TypesExtensions
    {
        public static Ad ToAd(this AdJson adJson) => new Ad(adJson.Id,
                            new Trader(adJson.Owner, adJson.ownerBalance, adJson.OwnerLastActivity, adJson.IsOwnerVerificated, adJson.OwnerTrusted),
                            new Paymethod(adJson.Paymethod.Id, adJson.Paymethod.Name),
                            new Rate(new Currency(adJson.Currency, null, CurrencyTypeEnum.Bank), new Currency(adJson.Cryptocurrency, null, CurrencyTypeEnum.Crypto), adJson.Rate),
                            new Limit(adJson.LimitCurrency.Min, adJson.LimitCurrency.Max, adJson.LimitCurrency.RealMax),
                            new Limit(adJson.LimitCryptocurrency.Min, adJson.LimitCryptocurrency.Max, adJson.LimitCryptocurrency.RealMax),
                            adJson.Type == "selling" ? AdTypeEnum.Selling : AdTypeEnum.Buying, adJson.SafeMode);
    }
}
