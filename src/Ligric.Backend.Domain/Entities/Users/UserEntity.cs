using Ligric.Backend.Domain.Base;
using System;

namespace Ligric.Backend.Domain.Entities.Users
{
    public class UserEntity : EntityBase
    {
        public virtual string? UserName { get; set; }

        public virtual string? Salt { get; set; }

        public virtual string? Password { get; set; }
    }
}
