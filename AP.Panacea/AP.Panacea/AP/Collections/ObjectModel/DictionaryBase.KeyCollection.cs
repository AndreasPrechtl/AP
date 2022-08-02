using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AP.Collections.ReadOnly;

namespace AP.Collections.ObjectModel
{
    public partial class DictionaryBase<TKey, TValue> 
    {
        public class KeyCollection : DictionaryKeyCollection<DictionaryBase<TKey, TValue>, TKey, TValue>
        {
            public KeyCollection(DictionaryBase<TKey, TValue> dictionary)
                : base(dictionary)
            { }

            public new KeyCollection Clone()
            {
                return (KeyCollection)this.OnClone();
            }
        }
    }
}
