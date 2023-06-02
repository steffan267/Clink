namespace Clink.Subscriber;

[AttributeUsage(AttributeTargets.Class)]
public class ConsumerGroupAttribute : Attribute
{
    public string Name { get; }

    public ConsumerGroupAttribute(string name)
    {
        Name = name;
    }
}
