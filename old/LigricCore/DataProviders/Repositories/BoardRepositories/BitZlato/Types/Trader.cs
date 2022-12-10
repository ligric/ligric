using System;

namespace BoardRepositories.BitZlato.Types
{
    public class Trader : IEquatable<Trader>
    {
        public string Name { get; }
        public decimal? Balance { get; }
        public long LastActivity { get; }
        public bool Verificated { get; }
        public bool Trusted { get; }

        private readonly int hash;

        public Trader(string name, decimal? balance, long lastActivity, bool verificated, bool trasted)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balance = balance;
            LastActivity = lastActivity;
            Verificated = verificated;
            Trusted = trasted;

            hash = Name.GetHashCode() ^ (Balance ?? decimal.Zero).GetHashCode() ^ LastActivity.GetHashCode() ^ Verificated.GetHashCode() ^ Trusted.GetHashCode();
        }

        public bool Equals(Trader other)
        {
            return Equals(other.Name, Name) && Balance == other.Balance && LastActivity == other.LastActivity && Verificated == other.Verificated && Trusted == other.Trusted;
        }

        public override bool Equals(object obj)
        {
            return obj is Trader trader && Equals(trader);
        }

        public override int GetHashCode() => hash;

    }
}