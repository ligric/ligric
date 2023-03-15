using System;
using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Domain.Entities
{
	public class UserEntity : EntityBase
	{
		public virtual string? UserName { get; set; }

		public virtual string? Salt { get; set; }

		public virtual string? Password { get; set; }
	}
}
