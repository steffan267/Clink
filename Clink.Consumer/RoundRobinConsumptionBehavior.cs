using Microsoft.Extensions.DependencyInjection;

namespace Clink.Subscriber;

public class RoundRobinConsumerBehavior : IConsumerBehavior
{
    private readonly List<IConsumer> _consumers;
    private readonly IServiceProvider _provider;

    public RoundRobinConsumerBehavior(List<IConsumer> consumers, IServiceProvider provider)
    {
        _consumers = consumers;
        _provider = provider;
    }
    
    public async Task Consume(CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested is not true)
        {
            foreach (var consumer in _consumers)
            {
                var handlers = _provider.GetServices<IEventProcessor>()
                    .Where( p => p.GetConsumerGroup() == consumer.GetType().Name);

                await consumer.Consume(handlers);
            }
        }
    }
}