namespace AP.UniformIdentifiers;

public interface IRemotableUri : IUri
{
    Host Host { get; }
    bool IsRemote { get; }
}
