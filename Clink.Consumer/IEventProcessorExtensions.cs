using System.Reflection;

namespace Clink.Subscriber;

public static class IEventProcessorExtensions
{
    public static string GetConsumerGroup(this IEventProcessor consumer)
    {
        var attribute = consumer.GetType().GetCustomAttribute<ConsumerGroupAttribute>();
        return attribute is null ? "All" : attribute.Name;
    }
}