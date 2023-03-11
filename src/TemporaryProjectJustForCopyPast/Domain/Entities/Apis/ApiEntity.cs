using Ligric.Domain.Base;

namespace Ligric.Domain.Entities.Apies
{
    public class ApiEntity : EntityBase
    {
        public virtual string? PrivateKey { get; set; }

        public virtual string? PublicKey { get; set; }
    }
}
