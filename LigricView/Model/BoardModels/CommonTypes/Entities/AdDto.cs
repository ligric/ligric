using System;

namespace BoardsCore.CommonTypes.Entities
{
    public class AdDto : IEquatable<AdDto>
    {
        public long Id { get; }

        private readonly int hash;
        public AdDto(long id)
        {
            Id = id;
            hash = Id.GetHashCode();
        }

        public bool Equals(AdDto other)
        {
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            return obj is AdDto ad && Equals(ad);
        }

        public override int GetHashCode() => hash;
    }
}
