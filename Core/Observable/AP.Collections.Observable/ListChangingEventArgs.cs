using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Observable.Collections;

public class ListChangingEventArgs<T> : CollectionChangingEventArgs<T>
{
    private readonly IDictionaryView<int, T> _newItems;
    private readonly IDictionaryView<int, T> _oldItems;

    public new ListChangeType Type => (ListChangeType)base.Type;
    public new IListView<T> Source => (IListView<T>)base.Source;

    public new IDictionaryView<int, T> NewItems => _newItems;
    public new IDictionaryView<int, T> OldItems => _oldItems;

    protected ListChangingEventArgs(IListView<T> source, IDictionaryView<int, T>? newItems = null, IDictionaryView<int, T>? oldItems = null, ListChangeType type = ListChangeType.Add)
        : base(source, newItems != null ? newItems.Values : ReadOnlyList<T>.Empty, oldItems != null ? oldItems.Values : ReadOnlyList<T>.Empty, (ChangeType)type)
    {
        _newItems = newItems ?? ReadOnlyDictionary<int, T>.Empty;
        _oldItems = oldItems ?? ReadOnlyDictionary<int, T>.Empty;
    }

    public static ListChangingEventArgs<T> Add(IListView<T> source, IDictionaryView<int, T> addedItems) => new(source, addedItems, null, ListChangeType.Add);

    public static ListChangingEventArgs<T> Insert(IListView<T> source, IDictionaryView<int, T> insertedItems) => new(source, insertedItems, null, ListChangeType.Insert);

    public static ListChangingEventArgs<T> Remove(IListView<T> source, IDictionaryView<int, T> removedItems) => new(source, null, removedItems, ListChangeType.Remove);

    public static ListChangingEventArgs<T> Move(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems) => new(source, newItems, oldItems, ListChangeType.Move);

    public static ListChangingEventArgs<T> Replace(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems) => new(source, newItems, oldItems, ListChangeType.Replace);

    public static ListChangingEventArgs<T> Clear(IListView<T> source, IDictionaryView<int, T> clearedItems) => new(source, null, clearedItems, ListChangeType.Clear);
}
