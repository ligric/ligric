using FluentNHibernate.Mapping;
using Ligric.Server.Domain.Base;
using Ligric.Server.Domain.Entities.UserApies;

namespace Ligric.Server.Domain.Mapping
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
