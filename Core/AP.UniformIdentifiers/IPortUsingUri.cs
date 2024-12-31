namespace AP.UniformIdentifiers;

public interface IPortUsingUri : IRemoteUri
{
    ushort? Port { get; }
    static ushort DefaultPort { get; }
}
