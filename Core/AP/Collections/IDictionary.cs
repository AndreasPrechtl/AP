using System.Collections.Generic;

namespace AP.Collections;

public interface IDictionary<TKey, TValue> : IDictionaryView<TKey, TValue>, System.Collections.Generic.IDictionary<TKey, TValue>
    where TKey : notnull
{
    new bool Add(TKey key, TValue value);
    void Add(params IEnumerable<KeyValuePair<TKey, TValue>> items);

    bool Update(TKey key, TValue value);
    void Update(params IEnumerable<KeyValuePair<TKey, TValue>> items);

    // new TValue this[TKey key] { get; set; }
    
    bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false);
    void Remove(params IEnumerable<TKey> keys);
    void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items, bool compareValues = false);
            
    // new void Clear();
}
