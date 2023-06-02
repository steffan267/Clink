namespace Clink.Core;

public static class EventSerializer
{
    private static IEventSerializer? _serializer;
    public static IEventSerializer? Serializer
    {
        get
        {
            if (_serializer is null)
            {
                throw new InvalidOperationException("Please set an IEventSerializer");
            }

            return _serializer;
        }
        set
        {
            if (_serializer is not null)
            {
                throw new InvalidOperationException("The serializer can't be changed after being set");
            }

            _serializer = value ?? throw new ArgumentNullException($"{nameof(Serializer)} can't be null");
        }
    }
}