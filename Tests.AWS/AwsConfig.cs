using Amazon;

namespace Tests.AWS;

public class AwsConfig
{
    public string? Endpoint { get; set; }
    public string? ConnectionString { get; init; }
    public string? AccessKey { get ; init; }
    public string? SecretKey { get ; init; }
    public string? NotificationServiceName { get; init; }
    public string? QueueServiceName { get; init; }
    public string? Region { get; init; }
}