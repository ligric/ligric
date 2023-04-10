using FluentNHibernate.Mapping;
using Ligric.Domain.Base;
using Ligric.Domain.Entities.Users;

namespace Ligric.Domain.Mapping
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
