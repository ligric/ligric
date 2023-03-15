using System.Collections.Generic;
using System.Linq;

namespace Ligric.Service.CryptoApisService.Domain.Entities
{
    public class OutboxMessage : EntityUnit
	{
        private OutboxMessage()
        {
        }

        public OutboxMessage(ApiEntity user)
        {
            User = @user;
        }

        public static OutboxMessage FromId(long id) => new() { Id = id };

        public new long? Id { get; private set; }

        public ApiEntity? User { get; private set; }

        public static IEnumerable<OutboxMessage> ToManyMessages(IEnumerable<ApiEntity> events)
            => events.Select(e => new OutboxMessage(e));
    }
}
