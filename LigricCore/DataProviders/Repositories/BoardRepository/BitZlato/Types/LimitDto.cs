using System;

namespace BoardRepository.BitZlato.Types
{
    public class LimitDto : IEquatable<LimitDto>
    {
        public decimal From { get; }
        public decimal To { get; }
        public decimal? RealMax { get; }

        private readonly int hash;

        public LimitDto(decimal from, decimal to, decimal? realMax)
        {
            From = from;
            To = to;
            RealMax = realMax;

            hash = From.GetHashCode() ^ To.GetHashCode() ^ (RealMax ?? decimal.Zero).GetHashCode();
        }

        public bool Equals(LimitDto other)
        {
            return From == other.From && To == other.To && RealMax == other.RealMax;
        }

        public override bool Equals(object obj)
        {
            return obj is LimitDto rate && Equals(rate);
        }

        public override int GetHashCode() => hash;
    }
}