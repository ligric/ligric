using Ligric.Server.Domain.Base;

namespace Ligric.Server.Domain.Entities.Apies
{
    public class ApiEntity : EntityBase
    {
        public virtual string? Name { get; set; }

        public virtual string? PrivateKey { get; set; }

        public virtual string? PublicKey { get; set; }
    }
}
