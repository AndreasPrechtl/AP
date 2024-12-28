namespace AP.UniformIdentifiers;

public interface IPortUsingUri : IRemotableUri
{
    ushort? Port { get; }
    static ushort DefaultPort { get; }
}
