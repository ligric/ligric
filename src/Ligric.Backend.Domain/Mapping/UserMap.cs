using FluentNHibernate.Mapping;
using Ligric.Backend.Domain.Base;
using Ligric.Backend.Domain.Entities.Users;

namespace Ligric.Backend.Domain.Mapping
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
