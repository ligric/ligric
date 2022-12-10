using System;

namespace BoardRepositories.BitZlato.Types
{
    public class Limit : IEquatable<Limit>
    {
        public decimal From { get; }
        public decimal To { get; }
        public decimal? RealMax { get; }

        private readonly int hash;

        public Limit(decimal from, decimal to, decimal? realMax)
        {
            From = from;
            To = to;
            RealMax = realMax;

            hash = From.GetHashCode() ^ To.GetHashCode() ^ (RealMax ?? decimal.Zero).GetHashCode();
        }

        public bool Equals(Limit other)
        {
            return From == other.From && To == other.To && RealMax == other.RealMax;
        }

        public override bool Equals(object obj)
        {
            return obj is Limit rate && Equals(rate);
        }

        public override int GetHashCode() => hash;
    }
}