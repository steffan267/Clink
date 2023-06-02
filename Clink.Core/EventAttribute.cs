namespace Clink.Core;

public class EventAttribute
{
    public EventAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; init; }
    public string Value { get; init; }
}