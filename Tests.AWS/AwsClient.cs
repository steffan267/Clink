using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Tests.AWS;

public class AwsClient
{
    public readonly string? NotificationServiceName;
    public readonly AmazonSimpleNotificationServiceClient SNS;
    public CreateTopicResponse Topic = null!;

    public readonly string? QueueServiceName;
    public readonly AmazonSQSClient SQS;
    public CreateQueueResponse Queue = null!;

    public AwsClient(AwsConfig config)
    {
        NotificationServiceName = config.NotificationServiceName;
        QueueServiceName = config.QueueServiceName;
        
        // TODO: Check wether or not "" vs "ignore" vs any at all has made a different.
        AWSCredentials awsCredentials = new BasicAWSCredentials("", "");

        SNS = new AmazonSimpleNotificationServiceClient(awsCredentials, new AmazonSimpleNotificationServiceConfig()
        {
            ServiceURL = config.ConnectionString,
        });

        SQS = new AmazonSQSClient(awsCredentials, new AmazonSQSConfig()
        {
            ServiceURL = config.ConnectionString,
        });
    }

    public async Task Initialize()
    {
        Topic = await InitTopicFifo();
        Queue = await InitQueueFifo();

        var temp = await SubscribeQueueToTopicAsync(Queue, Topic);

        // TODO: Move this into the method subscribeQueueToTopicAsync
        await SNS.SetSubscriptionAttributesAsync(new SetSubscriptionAttributesRequest()
        {
            SubscriptionArn = temp[Topic.TopicArn],
            AttributeName = "RawMessageDelivery",
            AttributeValue = "true"
        });
    }
    
    private async Task<CreateTopicResponse> InitTopicFifo() =>
        await SNS.CreateTopicAsync(new CreateTopicRequest()
        {
            Name = NotificationServiceName,
            Attributes = new Dictionary<string, string>()
            {
                ["FifoTopic"] = "true",
                ["ContentBasedDeduplication"] = "true",
            },
        });

    private async Task<CreateQueueResponse> InitQueueFifo() =>
        await SQS.CreateQueueAsync(new CreateQueueRequest()
        {
            QueueName = QueueServiceName,
            Attributes = new Dictionary<string, string>
            {
                ["FifoQueue"] = "true",
                ["ContentBasedDeduplication"] = "true",
            }
        });

    private async Task<IDictionary<string, string>> SubscribeQueueToTopicAsync(CreateQueueResponse queue, CreateTopicResponse topic) =>
        await SNS.SubscribeQueueToTopicsAsync(
            new List<string>() { topic.TopicArn },
            SQS,
            queue.QueueUrl);
}