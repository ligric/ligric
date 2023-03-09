using System;
using MediatR;

namespace Ligric.Backend.Domain.SeedWork
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}