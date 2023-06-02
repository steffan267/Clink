using Microsoft.Extensions.Logging;

namespace Clink.Core;

public interface IEvent
{
    /// <summary>
    /// Unique Identifier of the event from the event broker
    /// </summary>
    public string? Id { get; }
        
    /// <summary>
    /// Uniquely identifies the receive for handling the event. If multiple consumers consume the same event,
    /// they'll have the same <see cref="Id"/>, but different ReceiptHandles
    /// </summary>
    public string? ReceiptHandle { get; }
    
    /// <summary>
    /// Any "Metadata" 
    /// </summary>
    public Dictionary<string, EventAttribute> EventAttributes { get; }
    
    /// <summary>
    /// All variables will be pushed as a scoped property to the <see cref="ILogger{TCategoryName}"/> on receive
    /// </summary>
    public Dictionary<string, TraceAttribute> TraceAttributes { get; }
    
    /// <summary>
    /// The name identifies
    /// </summary>
    public string EventName { get; }
    
    public object? Payload { get; set; }
}

public interface IEvent<out TPayload> : IEvent
{
    /// <summary>
    /// Generic payload that is transmitted over the wire
    /// </summary>
    public new TPayload? Payload { get; }
}