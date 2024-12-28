using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
    public interface IDictionaryView<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>
    {
        new ICollection<TKey> Keys { get; }
        new ICollection<TValue> Values { get; }

        bool Contains(KeyValuePair<TKey, TValue> item, bool compareValues = false);
        bool Contains(TKey key, out TValue value);

        new bool ContainsKey(TKey key);
        bool ContainsValue(TValue value);

        IEnumerable<TValue> this[IEnumerable<TKey> keys] { get; }
    }

    public interface IDictionaryView
    {
        ICollection Keys { get; }
        ICollection Values { get; }

        bool Contains(DictionaryEntry item, bool compareValues = false);
        bool Contains(object key, out object value);

        bool ContainsKey(object key);
        bool ContainsValue(object value);

        IEnumerable this[IEnumerable keys] { get; }
    }

    public interface IDictionary<TKey, TValue> : IDictionaryView<TKey, TValue>, System.Collections.Generic.IDictionary<TKey, TValue>
    {
        new bool Add(TKey key, TValue value);
        new bool Add(KeyValuePair<TKey, TValue> item);        
        void Add(IEnumerable<KeyValuePair<TKey, TValue>> items);        
        
        // moved to DictionaryExtensions
        //void AddOrUpdate(TKey key, TValue value);
        //void AddOrUpdate(KeyValuePair<TKey, TValue> item);        
        //void AddOrUpdate(IEnumerable<KeyValuePair<TKey, TValue>> items);

        bool Update(TKey key, TValue value);
        bool Update(KeyValuePair<TKey, TValue> item);
        void Update(IEnumerable<KeyValuePair<TKey, TValue>> items);

        // new TValue this[TKey key] { get; set; }
        
        bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false);
        void Remove(IEnumerable<TKey> keys);
        void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items, bool compareValues = false);
                
        // new void Clear();
    }

    public interface IDictionary : IDictionaryView, System.Collections.IDictionary
    {
        new bool Add(object key, object value);
        bool Add(DictionaryEntry item);
        void Add(IEnumerable<DictionaryEntry> items);

        // moved to DictionaryExtensions
        //void AddOrUpdate(object key, object value);
        //void AddOrUpdate(DictionaryEntry item);        
        //void AddOrUpdate(IEnumerable<DictionaryEntry> items);

        bool Update(object key, object value);
        bool Update(DictionaryEntry item);
        void Update(IEnumerable<DictionaryEntry> items);

        // new TValue this[TKey key] { get; set; }

        bool Remove(DictionaryEntry item, bool compareValues = false);
        void Remove(IEnumerable keys);
        void Remove(IEnumerable<DictionaryEntry> items, bool compareValues = false);

        // new void Clear();
    }
}
