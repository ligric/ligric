using System;
using MediatR;

namespace Ligric.Domain.SeedWork
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}