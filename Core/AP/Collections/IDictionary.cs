using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
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
}
