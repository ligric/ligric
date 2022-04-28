using BoardsCore.CommonTypes.Entities;
using BoardsCommon.Enums;
using System;

namespace BoardsCore.BitZlato.Entities
{
    public class BitZlatoAdDto : AdDto, IEquatable<BitZlatoAdDto>
    {
        public TraderDto Trader { get; }
        public PaymethodDto Paymethod { get; }
        public RateDto Rate { get; }
        public LimitDto LimitCurrencyLeft { get; }
        public LimitDto LimitCurrencyRight { get; }
        public AdTypeEnum Type { get; }

        public bool SafeMode { get; }

        private readonly int hash;

        public BitZlatoAdDto(long id, TraderDto trader, PaymethodDto paymethod,
                              RateDto rate, LimitDto limitCurrencyLeft, LimitDto limitCurrencyRight, AdTypeEnum type, bool safeMode) :
            base(id)
        {
            Trader = trader ?? throw new ArgumentNullException(nameof(trader));
            Paymethod = paymethod ?? throw new ArgumentNullException(nameof(paymethod)); 
            Rate = rate ?? throw new ArgumentNullException(nameof(rate));
            LimitCurrencyLeft = limitCurrencyLeft ?? throw new ArgumentNullException(nameof(limitCurrencyLeft));
            LimitCurrencyRight = limitCurrencyRight ?? throw new ArgumentNullException(nameof(limitCurrencyRight));
            Type = type;
            SafeMode = safeMode;

            hash = Id.GetHashCode() ^ Trader.GetHashCode() ^ Paymethod.GetHashCode() ^ Rate.GetHashCode() ^
                   LimitCurrencyLeft.GetHashCode() ^ LimitCurrencyRight.GetHashCode() ^ Type.GetHashCode() ^ SafeMode.GetHashCode();
        }

        public bool Equals(BitZlatoAdDto other)
        {
            return other.Id == Id &&
                   Equals(other.Trader,Trader) &&
                   Equals(other.Paymethod, Paymethod) &&
                   Equals(other.Rate, Rate) &&
                   Equals(other.LimitCurrencyLeft, LimitCurrencyLeft) &&
                   Equals(other.LimitCurrencyRight, LimitCurrencyRight) &&
                   other.Type == Type &&
                   other.SafeMode == SafeMode;
        }

        public override bool Equals(object obj)
        {
            return obj is BitZlatoAdDto ad && Equals(ad);
        }

        public override int GetHashCode() => hash;
    }
}
