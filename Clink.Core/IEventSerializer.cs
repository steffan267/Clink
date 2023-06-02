namespace Clink.Core;

public interface IEventSerializer
{
    string Serialize(object obj);
    T Deserialize<T>(string data);
}

