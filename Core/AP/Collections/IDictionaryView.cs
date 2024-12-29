using System.Collections.Generic;

namespace AP.Collections;

public interface IDictionaryView<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    new ICollection<TKey> Keys { get; }
    new ICollection<TValue> Values { get; }

    bool Contains(KeyValuePair<TKey, TValue> item, bool compareValues = false);

    new bool TryGetValue(TKey key, out TValue? value);
    
    new bool ContainsKey(TKey key);
    bool ContainsValue(TValue value);

    IEnumerable<TValue> this[IEnumerable<TKey> keys] { get; }
}
