namespace AP.UniformIdentifiers;

public interface IAbsoluteOrRelativeUri : IHierarchicalUri
{
    bool IsAbsolute { get; }
}
