using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class DictionaryChangedEventArgs<TKey, TValue> : CollectionChangedEventArgs<KeyValuePair<TKey, TValue>>
    {
        public new DictionaryChangeType Type { get { return (DictionaryChangeType)base.Type; } }
        public new IDictionaryView<TKey, TValue> Source { get { return (IDictionaryView<TKey, TValue>)base.Source; } }
        public new IDictionaryView<TKey, TValue> NewItems { get { return (IDictionaryView<TKey, TValue>)base.NewItems; } }
        public new IDictionaryView<TKey, TValue> OldItems { get { return (IDictionaryView<TKey, TValue>)base.OldItems; } }
     
        protected DictionaryChangedEventArgs(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> newItems = null, IDictionaryView<TKey, TValue> oldItems = null, DictionaryChangeType type = DictionaryChangeType.Add)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static DictionaryChangedEventArgs<TKey, TValue> Add(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> addedItems)
        {
            return new DictionaryChangedEventArgs<TKey, TValue>(source, addedItems, null, DictionaryChangeType.Add);
        }

        public static DictionaryChangedEventArgs<TKey, TValue> Remove(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> removedItems)
        {
            return new DictionaryChangedEventArgs<TKey, TValue>(source, null, removedItems, DictionaryChangeType.Remove);
        }
              
        public static DictionaryChangedEventArgs<TKey, TValue> Update(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> originalItems, IDictionaryView<TKey, TValue> updatedItems)
        {
            return new DictionaryChangedEventArgs<TKey, TValue>(source, updatedItems, originalItems, DictionaryChangeType.Update);
        }
        
        public static DictionaryChangedEventArgs<TKey, TValue> Clear(IDictionaryView<TKey, TValue> source, IDictionaryView<TKey, TValue> clearedItems)
        {
            return new DictionaryChangedEventArgs<TKey, TValue>(source, null, clearedItems, DictionaryChangeType.Clear);
        }
    }
}
