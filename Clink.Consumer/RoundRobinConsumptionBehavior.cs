using Microsoft.Extensions.DependencyInjection;

namespace Clink.Subscriber;

public class RoundRobinConsumerBehavior : IConsumerBehavior
{
    private readonly List<IConsumer> _consumers;
    private readonly List<IEventProcessor> _processors;

    public RoundRobinConsumerBehavior(List<IConsumer> consumers, IEnumerable<IEventProcessor> eventProcessors)
    {
        _consumers = consumers;
        _processors = eventProcessors.ToList();
    }
    
    public async Task Consume(CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested is not true)
        {
            foreach (var consumer in _consumers)
            {
                var handlers = _processors.Where( p => p.GetConsumerGroup() == consumer.GetType().Name);

                await consumer.Consume(handlers);
            }
        }
    }
}