using FluentNHibernate.Mapping;
using Ligric.Service.AuthService.Domain.Entities;
using Ligric.Service.AuthService.Domain.Extensions;

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
