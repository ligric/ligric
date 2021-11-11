using BoardRepositories.Enums;
using BoardRepositories.Types;
using System;

namespace BoardRepositories.BitZlato.Types
{
    public class Ad : IEquatable<Ad>
    {
        public long Id { get; }
        public Trader Trader { get; }
        public Paymethod Paymethod { get; }
        public Rate Rate { get; }
        public Limit LimitCurrencyLeft { get; }
        public Limit LimitCurrencyRight { get; }
        public AdTypeEnum Type { get; }

        public bool SafeMode { get; }

        private readonly int hash;

        public Ad(long id, Trader trader, Paymethod paymethod, 
                              Rate rate, Limit limitCurrencyLeft, Limit limitCurrencyRight, AdTypeEnum type, bool safeMode)
        {
            Id = id; 
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

        public bool Equals(Ad other)
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
            return obj is Ad ad && Equals(ad);
        }

        public override int GetHashCode() => hash;
    }
}
