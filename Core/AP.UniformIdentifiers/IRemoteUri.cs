namespace AP.UniformIdentifiers;

public interface IRemoteUri : IUri
{
    Host Host { get; }
    bool IsRemote { get; }
}
