namespace AP.UniformIdentifiers;

public interface IFileUri : IHierarchicalUri
{
    string Extension { get; }
    //string Name { get; }
}
