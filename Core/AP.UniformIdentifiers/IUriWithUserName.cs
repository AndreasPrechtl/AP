namespace AP.UniformIdentifiers;

public interface IUriWithUserName : IUri
{
    string? UserName { get; }
}
