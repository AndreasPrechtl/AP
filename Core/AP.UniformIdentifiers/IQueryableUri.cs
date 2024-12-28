namespace AP.UniformIdentifiers;

public interface IQueryableUri : IUri
{
    UrlQuery Query { get; }
}
