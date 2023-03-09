using FluentNHibernate.Mapping;
using Ligric.Backend.Domain.Base;
using Ligric.Backend.Domain.Entities.Apies;

namespace Ligric.Backend.Domain.Mapping
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
