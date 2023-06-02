using Amazon.SQS.Model;
using Clink.Core;

namespace Clink.Aws.SqsConsumer;

public static class MessageExtensions
{
    public static IEvent ToUntypedEvent(this Message message)
    {
        Event @event = EventSerializer.Serializer!.Deserialize<Event>(message.Body);
        @event.Id = message.MessageId;
        @event.ReceiptHandle = message.ReceiptHandle;
        return @event;
    }
}