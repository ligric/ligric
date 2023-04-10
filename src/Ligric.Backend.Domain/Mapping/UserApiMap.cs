using FluentNHibernate.Mapping;
using Ligric.Backend.Domain.Base;
using Ligric.Backend.Domain.Entities.UserApies;

namespace Ligric.Backend.Domain.Mapping
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
