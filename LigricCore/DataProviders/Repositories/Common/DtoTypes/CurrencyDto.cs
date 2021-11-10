using Common.Enums;
using System;

namespace Common.DtoTypes
{
    public class CurrencyDto : IEquatable<CurrencyDto>
    {
        public string Name { get; }
        public string Symbol { get; }
        public CurrencyTypeEnum Type { get; }

        private readonly int hash;

        public CurrencyDto(string name, string symbol, CurrencyTypeEnum type)
        {; 
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Symbol = symbol ?? string.Empty;
            Type = type;

            hash = Name.GetHashCode() ^ Symbol.GetHashCode() ^ Type.GetHashCode();
        }

        public bool Equals(CurrencyDto other)
        {
            return Name == other.Name && Symbol == other.Symbol && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            return obj is CurrencyDto currency && Equals(currency);
        }

        public override int GetHashCode() => hash;
    }
}
