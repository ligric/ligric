using System;

namespace BoardsShared.BitZlato.Entities
{
    public class PaymethodDto : IEquatable<PaymethodDto>
    {
        public ushort Id { get; }
        public string Name { get; }

        private readonly int hash;

        public PaymethodDto(ushort id, string name)
        {
            Id = id; 
            Name = name ?? throw new ArgumentNullException(nameof(name));

            hash = Id ^ Name.GetHashCode();
        }

        public bool Equals(PaymethodDto other)
        {
            return Id == other.Id && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is PaymethodDto paymethod && Equals(paymethod);
        }

        public override int GetHashCode() => hash;
    }
}