namespace Clink.Core;

public abstract class OrderedEvent<TPayload> : Event<TPayload>, IOrderedEvent<TPayload>
{
    public string PartitionIdentifier { get; } = Guid.NewGuid().ToString();
    public string? DeduplicationId { get; } = Guid.NewGuid().ToString();
}