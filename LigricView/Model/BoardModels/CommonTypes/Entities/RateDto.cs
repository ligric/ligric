﻿using System;

namespace BoardsShared.BitZlato.Entities
{
    public class RateDto : IEquatable<RateDto>
    {
        public CurrencyDto LeftCurrency { get; }
        public CurrencyDto RightCurrency { get; }
        public decimal Value { get; }

        private readonly int hash;

        public RateDto(CurrencyDto leftCurrency, CurrencyDto rightCurrency, decimal value)
        {
            LeftCurrency = leftCurrency ?? throw new ArgumentNullException(nameof(leftCurrency));
            RightCurrency = rightCurrency ?? throw new ArgumentNullException(nameof(rightCurrency));
            Value = value;

            hash = LeftCurrency.GetHashCode() ^ RightCurrency.GetHashCode() ^ Value.GetHashCode();
        }

        public bool Equals(RateDto other)
        {
            return Equals(LeftCurrency, other.LeftCurrency) &&
                   Equals(RightCurrency, other.RightCurrency) &&
                   Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is RateDto rate && Equals(rate);
        }

        public override int GetHashCode() => hash;
    }
}