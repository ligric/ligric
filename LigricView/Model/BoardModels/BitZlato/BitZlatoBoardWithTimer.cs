using BoardModels.AbstractBoardNotifications.Abstractions;
using BoardModels.BitZlato.Entities;
using BoardRepositories.Abstractions;
using BoardRepositories.BitZlato;
using BoardRepositories.BitZlato.Types;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardModels.BitZlato
{
    public class BitZlatoBoardWithTimer : AbstractBoardWithTimerNotifications<long, BitZlatoAdDto>
    {
        private readonly AbstractBoardRepositoryWithTimer<long, Ad> repository;

        public BitZlatoBoardWithTimer(string boardName, string apiKey, string email, TimeSpan interval, IDictionary<string, string> filters, StateEnum defaultState = StateEnum.Stoped) :
            base(boardName, interval, filters, defaultState)
        {
            repository = new BitZlatoWithTimerRepository(apiKey, email, interval, filters, defaultState);

            repository.AdsChanged += Repository_AdsChanged;
        }

        private void Repository_AdsChanged(object sender, Common.EventArgs.NotifyDictionaryChangedEventArgs<long, Ad> e)
        {
            var newValues = e.NewValue;
            var oldValues = e.OldValue;

            switch (e.Action)
            {
                case Common.EventArgs.NotifyDictionaryChangedAction.Added:
                    Task.Run(async () => await syncMethod.WaitingAnotherMethodsAsync(e.Number, async () => await Task.Run(() => AdsRaiseActionAddValue(e.Key, e.NewValue.ConvertToBitZlatoAdDto()))));
                    break;
                case Common.EventArgs.NotifyDictionaryChangedAction.Removed:
                    Task.Run(async () => await syncMethod.WaitingAnotherMethodsAsync(e.Number, async () => await Task.Run(() => AdsRaiseActionRemoveValue(e.Key))));
                    break;
                case Common.EventArgs.NotifyDictionaryChangedAction.Changed:
                    Task.Run(async () => await syncMethod.WaitingAnotherMethodsAsync(e.Number, async () => await Task.Run(() => AdsRaiseActionSetValue(e.Key, e.NewValue.ConvertToBitZlatoAdDto()))));
                    break;
                case Common.EventArgs.NotifyDictionaryChangedAction.Cleared:
                    Task.Run(async () => await syncMethod.WaitingAnotherMethodsAsync(e.Number, async () => await Task.Run(() => AdsRaiseActionClear())));
                    break;
                case Common.EventArgs.NotifyDictionaryChangedAction.Initialized:
                    var newDictionary = new Dictionary<long, BitZlatoAdDto>();
                    foreach (var item in e.NewDictionary)
                    {
                        newDictionary.Add(item.Key, item.Value.ConvertToBitZlatoAdDto());
                    }
                    Task.Run(async () => await syncMethod.WaitingAnotherMethodsAsync(e.Number, async () => await Task.Run(() => AdsRaiseActionInitialized(newDictionary))));
                    break;
            }
        }
    }

    internal static class TypeExtantions
    {
        public static BitZlatoAdDto ConvertToBitZlatoAdDto(this Ad ad) => new BitZlatoAdDto(ad.Id,
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
