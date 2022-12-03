using FluentNHibernate.Mapping;
using Ligric.Server.Domain.Base;
using Ligric.Server.Domain.Entities;

namespace Ligric.Server.Domain.Mapping
{
    public class UserApiMap : ClassMap<UserApiEntity> 
    {
        public UserApiMap()
        {
            Table("[UserApis]");
            this.MapBase();

            Map(x => x.UserId);
            Map(x => x.ApiId);

            References(x => x.Api).Column("[ApiId]").ReadOnly();
        }
    }
}
