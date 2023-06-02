using Clink.Core;
using Microsoft.Extensions.Logging;

namespace Clink.Subscriber;

public interface IEventProcessor
{
    public bool IsMine(object @event);
    Task Handle(IEvent untyped);
}

public abstract class EventProcessor<TConsumer, TEvent,TPayload> : IEventProcessor where TConsumer : IConsumer where TEvent : IEvent<TPayload> where TPayload : class
{
    protected readonly ILogger<EventProcessor<TConsumer, TEvent,TPayload>> Logger;
    private const string EventReference = "Event";

    public IConsumer Consumer { get; }

    protected EventProcessor(TConsumer consumer, ILogger<EventProcessor<TConsumer,TEvent,TPayload>> logger)
    {
        Consumer = consumer;
        Logger = logger;
    }

    public async Task Handle(IEvent @event)
    {
        try
        {
            var transformed = Transform(@event);
            var implementorEvent = await BeforeProcess(transformed);
            await Process(implementorEvent);
            await BeforeAcknowledge(implementorEvent);
            await Consumer.Acknowledge(@event);
        }
        catch (Exception e)
        {
            e.Data.Add(EventReference, e);
            await OnFailure(e);
        }
    }

    protected virtual IEvent<TPayload> Transform(IEvent @event)
    {
        @event.Payload = @event.Payload as TPayload;
        return (IEvent<TPayload>)@event;
    }

    public abstract Task Process(IEvent<TPayload> @event);

    public virtual Task<IEvent<TPayload>> BeforeProcess(IEvent<TPayload> @event)
    {
        return Task.FromResult(@event);
    }
    
    public virtual Task BeforeAcknowledge(IEvent<TPayload> @event)
    {
        return Task.FromResult(@event);
    }
    
    public virtual async Task OnFailure(Exception exception)
    {
        await Consumer.Acknowledge((IEvent)exception.Data[EventReference]!);
    }

    protected virtual string GetNameOfEvent()
    {
        return nameof(TEvent);
    }
    
    public bool IsMine(object message)
    {
        try
        {
            if (message is IEvent e)
            {
                return e.EventName == GetNameOfEvent();
            }
        }
        catch
        {
            //TODO: log
        }
        return false;
    }
}