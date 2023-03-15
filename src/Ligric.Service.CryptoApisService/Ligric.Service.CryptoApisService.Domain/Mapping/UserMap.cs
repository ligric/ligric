using FluentNHibernate.Mapping;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Extensions;

namespace Ligric.Service.CryptoApisService.Domain.Mapping
{
	public class UserMap : ClassMap<UserEntity>
	{
		public UserMap()
		{
			Table("[Users]");
			this.MapBase();

			Map(x => x.UserName);
			Map(x => x.Password);
			Map(x => x.Salt);
		}
	}
}
