using Ligric.Server.Domain.Base;

namespace Ligric.Server.Domain.Entities
{
    public class UserApiEntity : EntityBase
    {
        public virtual long? UserId { get; set; }

        public virtual long? ApiId { get; set; }

        public virtual ApiEntity? Api { get; set; }
    }
}
