namespace AP.UniformIdentifiers;

public interface IFragmentableUri : IUri        
{
    UrlFragments Fragments { get; }
}
