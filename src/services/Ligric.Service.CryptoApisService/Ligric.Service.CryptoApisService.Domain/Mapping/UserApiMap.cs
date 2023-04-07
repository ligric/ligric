using FluentNHibernate.Mapping;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Extensions;

namespace Ligric.Service.CryptoApisService.Domain.Mapping
{
	public class UserApiMap : ClassMap<UserApiEntity>
	{
		public UserApiMap()
		{
			Table("[UserAPIs]");
			this.MapBase();

			Map(x => x.UserId);
			Map(x => x.ApiId);
			Map(x => x.Name);
			Map(x => x.Permissions);

			References(x => x.Api).Column("[ApiId]").ReadOnly();
		}
	}
}
