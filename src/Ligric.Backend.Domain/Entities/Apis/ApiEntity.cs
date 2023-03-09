using Ligric.Backend.Domain.Base;

namespace Ligric.Backend.Domain.Entities.Apies
{
    public class ApiEntity : EntityBase
    {
        public virtual string? PrivateKey { get; set; }

        public virtual string? PublicKey { get; set; }
    }
}
