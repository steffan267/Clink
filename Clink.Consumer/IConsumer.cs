using Clink.Core;

namespace Clink.Subscriber;

public interface IConsumer
{
    Task Consume(IEnumerable<IEventProcessor> eventProcessors);
    Task<bool> Acknowledge(IEvent @event);
}