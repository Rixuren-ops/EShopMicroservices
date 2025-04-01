using Ordering.Domain.Abstractions;
using Ordering.Domain.Models;

namespace Ordering.Domain.Events
{
    public class OrderUpdatedEvent(Order order) : IDomainEvent;
}
