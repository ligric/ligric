using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Domain.Entities
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
