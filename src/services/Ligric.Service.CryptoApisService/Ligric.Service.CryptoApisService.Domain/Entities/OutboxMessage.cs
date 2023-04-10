using System.Collections.Generic;
using System.Linq;

namespace Ligric.Service.CryptoApisService.Domain.Entities
{
    public class OutboxMessage : EntityUnit
	{
        private OutboxMessage()
        {
        }

        public OutboxMessage(UserApiEntity userApi)
        {
			UserApi = userApi;
        }

        public static OutboxMessage FromId(long id) => new() { Id = id };

        public new long? Id { get; private set; }

        public UserApiEntity? UserApi { get; private set; }

        public static IEnumerable<OutboxMessage> ToManyMessages(IEnumerable<UserApiEntity> events)
            => events.Select(e => new OutboxMessage(e));
    }
}
