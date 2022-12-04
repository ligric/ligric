using Ligric.Server.Domain.Base;
using System;

namespace Ligric.Server.Domain.Entities.Users
{
    public class UserEntity : EntityBase
    {
        public virtual string? UserName { get; set; }

        public virtual string? Salt { get; set; }

        public virtual string? Password { get; set; }
    }
}
