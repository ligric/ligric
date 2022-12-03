using FluentNHibernate.Mapping;
using Ligric.Server.Domain.Base;
using Ligric.Server.Domain.Entities.Users;

namespace Ligric.Server.Domain.Mapping
{
    public class UserMap : ClassMap<UserEntity> 
    {
        public UserMap()
        {
            Table("[Users]");
            this.MapBase();

            Map(x => x.UserName);
            Map(x => x.Password);
        }
    }
}
