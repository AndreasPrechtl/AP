using System.Collections.Generic;
using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Observable.Collections;

public class DictionaryChangedEventArgs<TKey, TValue> : CollectionChangedEventArgs<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    public new DictionaryChangeType Type => (DictionaryChangeType)base.Type;
    public new IDictionaryView<TKey, TValue> Source => (IDictionaryView<TKey, TValue>)base.Source;
    public new IDictionaryView<TKey, TValue> NewItems => (IDictionaryView<TKey, TValue>)base.NewItems;
    public new IDictionaryView<TKey, TValue> OldItems => (IDictionaryView<TKey, TValue>)base.OldItems;

    protected DictionaryChangedEventArgs(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> newItems, IDictionaryView<TKey, TValue> oldItems, DictionaryChangeType type = DictionaryChangeType.Add)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static DictionaryChangedEventArgs<TKey, TValue> Add(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> addedItems) => new(source, addedItems, ReadOnlyDictionary<TKey, TValue>.Empty, DictionaryChangeType.Add);

    public static DictionaryChangedEventArgs<TKey, TValue> Remove(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> removedItems) => new(source, ReadOnlyDictionary<TKey, TValue>.Empty, removedItems, DictionaryChangeType.Remove);

    public static DictionaryChangedEventArgs<TKey, TValue> Update(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> originalItems, IDictionaryView<TKey, TValue> updatedItems) => new(source, updatedItems, originalItems, DictionaryChangeType.Update);

    public static DictionaryChangedEventArgs<TKey, TValue> Clear(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> clearedItems) => new(source, ReadOnlyDictionary<TKey, TValue>.Empty, clearedItems, DictionaryChangeType.Clear);
}
