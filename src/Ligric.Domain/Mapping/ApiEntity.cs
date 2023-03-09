using FluentNHibernate.Mapping;
using Ligric.Domain.Base;
using Ligric.Domain.Entities.Apies;

namespace Ligric.Domain.Mapping
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
