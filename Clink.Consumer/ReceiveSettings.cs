namespace Clink.Subscriber;

public class ReceiveSettings
{
    public string QueueUrl { get; init; } = null!;
    public int MaxReceiveCount { get; init; } = 1;

    public bool AcknowledgeUnknownEvents { get; init; } = false;
}