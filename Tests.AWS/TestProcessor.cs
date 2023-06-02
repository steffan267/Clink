using Clink.Aws.SqsConsumer;
using Clink.Core;
using Clink.Subscriber;
using Microsoft.Extensions.Logging;

namespace Tests.AWS;

[ConsumerGroup(nameof(SqsConsumer))]
public class TestProcessor : EventProcessor<ISqsConsumer,TestEvent,TestPayload>
{
    public TestProcessor(ISqsConsumer consumer, ILogger<EventProcessor<ISqsConsumer, TestEvent, TestPayload>> logger) : base(consumer, logger)
    {
    }

    public override Task Process(IEvent<TestPayload> @event)
    {
        throw new NotImplementedException();
    }
}

public class TestEvent : Event<TestPayload>
{
    public override string EventName => nameof(TestEvent);
}

public class TestPayload
{
    
}