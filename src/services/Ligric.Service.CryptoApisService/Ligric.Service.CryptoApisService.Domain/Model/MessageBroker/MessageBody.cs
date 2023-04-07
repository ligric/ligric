using System;
using MediatR;

namespace Ligric.Service.CryptoApisService.Domain.Model.MessageBroker
{
    public class MessageBody<T> : IRequest<bool>, IMessageBody<T>
    {
        public string? AggregateId { get; set; }
        public long Sequence { get; set; }
        public T? Data { get; set; }
        public string? Type { get; set; }
        public DateTime DateTime { get; set; }
        public int? Version { get; set; }
    }
}
