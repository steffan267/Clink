namespace Clink.Core;

public interface IOrderedEvent<out TPayload> : IEvent<TPayload>
{
    public string PartitionIdentifier { get; }
    
    public string? DeduplicationId { get; }
}