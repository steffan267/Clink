using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;
using Clink.Core;
using Clink.Subscriber;
using Microsoft.Extensions.Logging;

namespace Clink.Aws.SqsConsumer;

public class SqsConsumer : ISqsConsumer
{
        private readonly IAmazonSQS _sqsClient;
        private readonly ILogger<SqsConsumer> _logger;
        private readonly ReceiveSettings _settings;
        private const string QueueUrl = "QueueUrl";

        public SqsConsumer(
            IAmazonSQS sqsClient,
            ILogger<SqsConsumer> logger,
            ReceiveSettings settings
        )
        {
            _settings = settings;
            _sqsClient = sqsClient;
            _logger = logger;
        }

        public async Task Consume(IEnumerable<IEventProcessor> eventProcessors)
        {
            try
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _settings.QueueUrl,
                    MaxNumberOfMessages = _settings.MaxReceiveCount,
                    MessageAttributeNames = new List<string>(){
                        "All"   
                    }
                };
                
                eventProcessors = eventProcessors.ToList();

                var response = await _sqsClient.ReceiveMessageAsync(request);

                foreach (var m in response.Messages)
                {
                    m.Attributes.Add(QueueUrl, _settings.QueueUrl);

                    var untyped = m.ToUntypedEvent();
                    
                    var processor = eventProcessors.FirstOrDefault(e => e.IsMine(untyped));

                    if (processor is not null)
                    {
                        await processor.Handle(untyped);
                    }
                    else
                    {
                        if (_settings.AcknowledgeUnknownEvents)
                        {
                            await Acknowledge(untyped);
                        }

                        _logger.LogError("No Handler for type {Event} in {Consumer}", untyped.EventName, this.GetType().Namespace);
                    }
                }
                
                //remember to add EventAttribute called "QueueUrl"
            }
            catch(Exception exception)
            {
                _logger.LogError(exception,"Something went wrong when getting messages from the queue");
            }
        }

        public async Task<bool> Acknowledge(IEvent @event)
        {
            var deleteRequest = new DeleteMessageRequest
            {
                QueueUrl = @event.EventAttributes[QueueUrl].Value,
                ReceiptHandle = @event.Id,
            };

            var deleted = await _sqsClient.DeleteMessageAsync(deleteRequest);
            return deleted.HttpStatusCode == HttpStatusCode.OK;
        }
}