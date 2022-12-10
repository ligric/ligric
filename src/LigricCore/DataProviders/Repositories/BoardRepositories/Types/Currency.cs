using BoardRepositories.Enums;
using System;

namespace BoardRepositories.Types
{
    public class Currency : IEquatable<Currency>
    {
        public string Name { get; }
        public string Symbol { get; }
        public CurrencyTypeEnum Type { get; }

        private readonly int hash;

        public Currency(string name, string symbol, CurrencyTypeEnum type)
        { 
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Symbol = symbol ?? string.Empty;
            Type = type;

            hash = Name.GetHashCode() ^ Symbol.GetHashCode() ^ Type.GetHashCode();
        }

        public bool Equals(Currency other)
        {
            return Equals(other.Name, Name) && Equals(other.Symbol, Symbol) && Equals(other.Type, Type);
        }

        public override bool Equals(object obj)
        {
            return obj is Currency currency && Equals(currency);
        }

        public override int GetHashCode() => hash;
    }
}
