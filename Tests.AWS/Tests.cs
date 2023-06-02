using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Clink.Aws.SqsConsumer;
using Clink.Subscriber;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests.AWS;

public class Tests
{
    
    [Test]
    public async Task CreateServiceContainer()
    {
        using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IAmazonSQS>(SetupFixture.AwsClient.SQS);
                services.AddSingleton<IAmazonSimpleNotificationService>(SetupFixture.AwsClient.SNS);
                
                services.AddScoped<ISqsConsumer, SqsConsumer>();
                services.AddScoped<IEventProcessor, EventProcessor<SqsConsumer, TestEvent, TestPayload>>();
            })
            .Build();


        await host.StartAsync();

        var sns = host.Services.GetRequiredService<IAmazonSimpleNotificationService>();
        
        var publishRequest = new PublishRequest()
        {
            Message = "Message",
            MessageGroupId = "A",
            TopicArn = SetupFixture.AwsClient.Topic.TopicArn,
            MessageDeduplicationId = Guid.NewGuid().ToString()
        };

        await sns.PublishAsync(publishRequest);


        Console.WriteLine("Got here");
        await host.StopAsync();
    }
}