using FluentNHibernate.Mapping;
using Ligric.Service.CryptoApisService.Domain.Entities;
using Ligric.Service.CryptoApisService.Domain.Extensions;

namespace Ligric.Service.CryptoApisService.Domain.Mapping
{
	public class ApiMap : ClassMap<ApiEntity>
	{
		public ApiMap()
		{
			Table("[APIs]");
			this.MapBase();

			Map(x => x.PublicKey);
			Map(x => x.PrivateKey);
		}
	}
}
