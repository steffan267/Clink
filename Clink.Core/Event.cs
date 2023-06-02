using Clink.Core;

namespace Clink.Core;

public abstract class Event<TPayload> : Event, IEvent<TPayload>
{
    public new TPayload? Payload { get; set; }
}

public abstract class Event : IEvent
{
    public string? Id { get; set; } 
    public string? ReceiptHandle { get; set; }
    public Dictionary<string, EventAttribute> EventAttributes { get; } = new Dictionary<string, EventAttribute>();
    public Dictionary<string, TraceAttribute> TraceAttributes { get; } = new Dictionary<string, TraceAttribute>();
    public abstract string EventName { get; }
    public object? Payload { get; set; }
}