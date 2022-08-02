namespace AP.UI
{
    public interface IOverviewPage<T> : IPage
    {
        IPagedViewModel<T> Model { get; }
    }

    public interface IOverviewPage<T, out TNavigation> : IPage
    {
        IPagedViewModel<T, TNavigation> Model { get; }
    }
}