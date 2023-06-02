namespace Clink.Core;

public sealed class TraceAttribute
{
    public TraceAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }
    
    public readonly string Key;
    public readonly string Value;
}