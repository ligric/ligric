using Ligric.Domain.SeedWork;

namespace Ligric.Domain.Users
{
    public class UserRegisteredEvent : DomainEventBase
    {
        public UserId UserId { get; }

        public UserRegisteredEvent(UserId userId)
        {
            this.UserId = userId;
        }
    }
}