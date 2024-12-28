namespace AP.UniformIdentifiers;

public interface IHierarchicalUri : IUri
{
    string Path { get; }
    string Name { get; }

    IHierarchicalUri? Parent { get; }
    bool HasParent { get; }
}
