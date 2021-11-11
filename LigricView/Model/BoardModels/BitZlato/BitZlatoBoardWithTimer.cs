using BoardModels.AbstractBoardNotifications.Abstractions;
using BoardModels.BitZlato.Entities;
using BoardRepositories.Abstractions;
using BoardRepositories.BitZlato;
using BoardRepositories.BitZlato.Types;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BoardModels.BitZlato
{
    public class BitZlatoBoardWithTimer : AbstractBoardWithTimerNotifications<BitZlatoAdDto>
    {
        private readonly AbstractBoardRepositoryWithTimer<Ad> repository;
        
        public BitZlatoBoardWithTimer(string boardName, string apiKey, string email, TimeSpan interval, IDictionary<string, string> filters, StateEnum defaultState = StateEnum.Stoped) :
            base(boardName, interval, filters, defaultState)
        {
            repository = new BitZlatoWithTimerRepository(apiKey, email, interval, filters, defaultState);
 
            repository.AdsChanged += Repository_AdsChanged;
        }

        private void Repository_AdsChanged(object sender, Common.EventArgs.NotifyEnumerableChangedEventArgs<Ad> e)
        {
            var newValues = e.NewValues;
            var oldValues = e.OldValues;

            switch (e.Action)
            {
                case Common.EventArgs.NotifyEnumumerableChangedAction.Added:
                    foreach (var item in newValues)
                    {
                        AddAdAndSendAction(item.Id, item.ConvertToKek());
                    }
                    break;
                case Common.EventArgs.NotifyEnumumerableChangedAction.Changed:
                    foreach (var item in newValues)
                    {
                        SetAdAndSendAction(item.Id, item.ConvertToKek());
                    }
                    break;
                case Common.EventArgs.NotifyEnumumerableChangedAction.Removed:
                    foreach (var item in oldValues)
                    {
                        RemoveAdAndSendAction(item.Id);
                    }
                    break;
                case Common.EventArgs.NotifyEnumumerableChangedAction.Reset:
                    ClearAdsAndSendAction();
                    foreach (var item in e.NewValues)
                    {
                        AddAdAndSendAction(item.Id, item.ConvertToKek());
                    }
                    break;
                default:
                    break;
            }
        }



    }

    internal static class TypeExtantions
    {
        public static BitZlatoAdDto ConvertToKek(this Ad ad) => new BitZlatoAdDto(ad.Id,
            new TraderDto(ad.Trader.Name, ad.Trader.Balance, ad.Trader.LastActivity, ad.Trader.Verificated, ad.Trader.Trusted),
            new PaymethodDto(ad.Paymethod.Id, ad.Paymethod.Name),
            new RateDto(new CurrencyDto(ad.Rate.LeftCurrency.Name, ad.Rate.LeftCurrency.Symbol, (CommonTypes.Enums.CurrencyTypeEnum)ad.Rate.LeftCurrency.Type),
                        new CurrencyDto(ad.Rate.RightCurrency.Name, ad.Rate.RightCurrency.Symbol, (CommonTypes.Enums.CurrencyTypeEnum)ad.Rate.RightCurrency.Type),
                        ad.Rate.Value),
            new LimitDto(ad.LimitCurrencyLeft.From, ad.LimitCurrencyLeft.To, ad.LimitCurrencyLeft.RealMax),
            new LimitDto(ad.LimitCurrencyRight.From, ad.LimitCurrencyRight.To, ad.LimitCurrencyRight.RealMax),
            (CommonTypes.Enums.AdTypeEnum)ad.Type, ad.SafeMode);
    }
}
