namespace AP.UniformIdentifiers;

/// <summary>
/// Experimental - not sure if there is such a thing
/// </summary>
public interface IEncryptedUri : IUri
{
    string EncryptionMode { get; }
}
