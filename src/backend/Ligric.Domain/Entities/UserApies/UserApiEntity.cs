using Ligric.Domain.Base;
using Ligric.Domain.Entities.Apies;

namespace Ligric.Domain.Entities.UserApies
{
    public class UserApiEntity : EntityBase
    {
        public virtual long? UserId { get; set; }

        public virtual long? ApiId { get; set; }

        public virtual string? Name { get; set; }

		public virtual int? Permissions { get; set; }

        public virtual ApiEntity? Api { get; set; }
    }
}
