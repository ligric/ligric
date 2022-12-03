using Ligric.Server.Domain.Base;
using System;

namespace Ligric.Server.Domain.Entities
{
    public class UserEntity : EntityBase
    {
        public virtual string? UserName { get; set; }

        public virtual string? Password { get; set; }
    }
}
