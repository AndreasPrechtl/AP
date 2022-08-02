using System.Collections.Generic;
using AP.Collections;

namespace AP.Observable.Collections
{
    public class DictionaryChangingEventArgs<TKey, TValue> : CollectionChangingEventArgs<KeyValuePair<TKey, TValue>>
    {
        public new DictionaryChangeType Type { get { return (DictionaryChangeType)base.Type; } }
        public new IDictionaryView<TKey, TValue> Source { get { return (IDictionaryView<TKey, TValue>)base.Source; } }
        public new IDictionaryView<TKey, TValue> NewItems { get { return (IDictionaryView<TKey, TValue>)base.NewItems; } }
        public new IDictionaryView<TKey, TValue> OldItems { get { return (IDictionaryView<TKey, TValue>)base.OldItems; } }

        protected DictionaryChangingEventArgs(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> newItems = null, IDictionaryView<TKey, TValue> oldItems = null, DictionaryChangeType type = DictionaryChangeType.Add)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static DictionaryChangingEventArgs<TKey, TValue> Add(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> addedItems)
        {
            return new DictionaryChangingEventArgs<TKey, TValue>(source, addedItems, null, DictionaryChangeType.Add);
        }

        public static DictionaryChangingEventArgs<TKey, TValue> Remove(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> removedItems)
        {
            return new DictionaryChangingEventArgs<TKey, TValue>(source, null, removedItems, DictionaryChangeType.Remove);
        }

        public static DictionaryChangingEventArgs<TKey, TValue> Update(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> originalItems, IDictionaryView<TKey, TValue> updatedItems)
        {
            return new DictionaryChangingEventArgs<TKey, TValue>(source, updatedItems, originalItems, DictionaryChangeType.Update);
        }

        public static DictionaryChangingEventArgs<TKey, TValue> Clear(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> clearedItems)
        {
            return new DictionaryChangingEventArgs<TKey, TValue>(source, null, clearedItems, DictionaryChangeType.Clear);
        }
    }
}