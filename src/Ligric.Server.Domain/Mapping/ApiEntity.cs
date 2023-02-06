using FluentNHibernate.Mapping;
using Ligric.Server.Domain.Base;
using Ligric.Server.Domain.Entities.Apies;

namespace Ligric.Server.Domain.Mapping
{
    public class ApiMap : ClassMap<ApiEntity> 
    {
        public ApiMap()
        {
            Table("[Apies]");
            this.MapBase();

            Map(x => x.Name);
            Map(x => x.PublicKey);
            Map(x => x.PrivateKey);
        }
    }
}
