using System;
using Ligric.Domain.SeedWork;

namespace Ligric.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}