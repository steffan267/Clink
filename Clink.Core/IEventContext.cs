namespace Clink.Core
{
    /// <summary>
    /// Responsible for passing a long the command context of which the current scope is being executed
    /// </summary>
    public interface IEventContext
    {
        string? CorrelationId { get; }
    }
}