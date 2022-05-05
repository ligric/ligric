﻿using System;

namespace BoardRepositories.Types
{
    public class Rate : IEquatable<Rate>
    {
        public Currency LeftCurrency { get; }
        public Currency RightCurrency { get; }
        public decimal Value { get; }

        private readonly int hash;

        public Rate(Currency leftCurrency, Currency rightCurrency, decimal value)
        {
            LeftCurrency = leftCurrency ?? throw new ArgumentNullException(nameof(leftCurrency));
            RightCurrency = rightCurrency ?? throw new ArgumentNullException(nameof(rightCurrency));
            Value = value;

            hash = LeftCurrency.GetHashCode() ^ RightCurrency.GetHashCode() ^ Value.GetHashCode();
        }

        public bool Equals(Rate other)
        {
            return Equals(other.LeftCurrency, LeftCurrency) &&
                   Equals(other.RightCurrency, RightCurrency) &&
                   Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Rate rate && Equals(rate);
        }

        public override int GetHashCode() => hash;
    }
}