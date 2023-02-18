using Ligric.Server.Domain.Base;
using Ligric.Server.Domain.Entities.Apies;

namespace Ligric.Server.Domain.Entities.UserApies
{
    public class UserApiEntity : EntityBase
    {
        public virtual long? UserId { get; set; }

        public virtual long? ApiId { get; set; }

		public virtual int? Permissions { get; set; }

        public virtual ApiEntity? Api { get; set; }
    }
}
