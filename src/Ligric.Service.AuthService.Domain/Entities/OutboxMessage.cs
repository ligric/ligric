using System.Collections.Generic;
using System.Linq;

namespace Ligric.Service.AuthService.Domain.Entities
{
    public class OutboxMessage
	{
        private OutboxMessage()
        {
        }

        public OutboxMessage(UserEntity user)
        {
            User = @user;
        }

        public static OutboxMessage FromId(long id) => new() { Id = id };

        public long? Id { get; private set; }

        public UserEntity? User { get; private set; }

        public static IEnumerable<OutboxMessage> ToManyMessages(IEnumerable<UserEntity> events)
            => events.Select(e => new OutboxMessage(e));
    }
}
