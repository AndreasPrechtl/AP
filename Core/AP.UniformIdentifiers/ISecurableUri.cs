namespace AP.UniformIdentifiers;

public interface ISecurableUri : IRemoteUri
{
    bool IsSecure { get; }
}
