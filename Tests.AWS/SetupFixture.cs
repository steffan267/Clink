using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Clink.Aws.SqsConsumer;
using Clink.Subscriber;
using Testcontainers.LocalStack;
using AwsClient = Tests.AWS.AwsClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests.AWS;

[SetUpFixture]
public class SetupFixture
{
    public static readonly LocalStackContainer LocalStack = new LocalStackBuilder().Build();
    
    public static AwsClient AwsClient = null!;
    public AwsConfig AwsConfig = null!;

    [OneTimeSetUp]
    public async Task SetupAsync()
    {
        var localstackContainer = LocalStack.StartAsync();

        await Task.WhenAll(localstackContainer);
        
        AwsConfig = new AwsConfig()
        {
            ConnectionString = LocalStack.GetConnectionString(),
            NotificationServiceName = "SnsTest",
            QueueServiceName = "SqsTest",
        };

        AwsClient = new AwsClient(AwsConfig);
        await AwsClient.Initialize();
    }
    
    [OneTimeTearDown]
    public async Task DisposeAsync()
    {
        await LocalStack.DisposeAsync();
    }
}