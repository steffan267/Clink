using System.Reflection;

namespace Clink.Subscriber;

public interface IConsumerBehavior
{
    public Task Consume(CancellationToken cancellationToken);
}