using System;

namespace BoardRepositories.Types
{
    public class Paymethod : IEquatable<Paymethod>
    {
        public ushort Id { get; }
        public string Name { get; }

        private readonly int hash;

        public Paymethod(ushort id, string name)
        {
            Id = id; 
            Name = name ?? throw new ArgumentNullException(nameof(name));

            hash = Id ^ Name.GetHashCode();
        }

        public bool Equals(Paymethod other)
        {
            return Id == other.Id && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Paymethod paymethod && Equals(paymethod);
        }

        public override int GetHashCode() => hash;
    }
}