namespace Clink.Core;

/// <summary>
/// This property will automatically be added to the event as "EventCorrelationId"
/// </summary>
public class EventContext : IEventContext
{
    public string? CorrelationId { get; internal set; }
}