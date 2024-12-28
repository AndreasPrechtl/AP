namespace AP.UniformIdentifiers;

public interface ISecurableUri : IRemotableUri
{
    bool IsSecure { get; }
}
