//using System.Reflection;
//using AP.Collections.ReadOnly;
//using AP.ComponentModel;
//using System.Dynamic;
//using AP.Reflection;

//namespace AP.Collections.Specialized
//{
//    public sealed class ObjectDictionary<TValue> : System.Dynamic.DynamicObject, IDictionary<string, object>, IDictionaryView<string, object>, IDictionary, IDictionaryView, IWrapper<TValue>
//    {
//        private readonly dynamic _value;

//        public TValue Value { get { return _value; } }

//        public ObjectDictionary(TValue obj) 
//        {
//            _value = obj;
//        }
    
//        TValue IWrapper<TValue>.Value
//        {
//            get
//            {
//                return this.Value;
//            }
//        }

//        #region IDictionary<string,object> Members

//        private const BindingFlags _flags = BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.FlattenHierarchy;

//        public class KeyCollection : ReadOnlyCollection<object>
//        {
//            protected internal KeyCollection(ICollectionView<TValue> inner) : base(inner)
//            {
//            }
//        }
        
//        private KeyCollection _keys;
//        public ICollectionView<string> Keys
//        {
//            get { return _keys = new KeyCollection(new Reflect.GetMembers<TValue>(_flags).Select()); }
//        }

//        public ICollectionView<object> Values
//        {
//            get { throw new System.NotImplementedException(); }
//        }

//        public bool ContainsValue(object value)
//        {
//            throw new System.NotImplementedException();
//        }

//        #endregion

//        #region IDictionary<string,object> Members

//        public void Add(string key, object value)
//        {
//            throw new System.NotImplementedException();
//        }

//        public bool ContainsKey(string key)
//        {
//            throw new System.NotImplementedException();
//        }

//        System.Collections.Generic.ICollection<string> System.Collections.Generic.IDictionary<string, object>.Keys
//        {
//            get { throw new System.NotImplementedException(); }
//        }

//        public bool Remove(string key)
//        {
//            throw new System.NotImplementedException();
//        }

//        public bool TryGetValue(string key, out object value)
//        {
//            throw new System.NotImplementedException();
//        }

//        System.Collections.Generic.ICollection<object> System.Collections.Generic.IDictionary<string, object>.Values
//        {
//            get { throw new System.NotImplementedException(); }
//        }

//        public object this[string key]
//        {
//            get
//            {
//                throw new System.NotImplementedException();
//            }
//            set
//            {
//                throw new System.NotImplementedException();
//            }
//        }

//        #endregion

//        #region ICollection<KeyValuePair<string,object>> Members

//        public void Add(System.Collections.Generic.KeyValuePair<string, object> item)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void Clear()
//        {
//            throw new System.NotImplementedException();
//        }

//        public bool Contains(System.Collections.Generic.KeyValuePair<string, object> item)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void CopyTo(System.Collections.Generic.KeyValuePair<string, object>[] array, int arrayIndex)
//        {
//            throw new System.NotImplementedException();
//        }

//        public int Count
//        {
//            get { throw new System.NotImplementedException(); }
//        }

//        public bool IsReadOnly
//        {
//            get { throw new System.NotImplementedException(); }
//        }

//        public bool Remove(System.Collections.Generic.KeyValuePair<string, object> item)
//        {
//            throw new System.NotImplementedException();
//        }

//        #endregion

//        #region IEnumerable<KeyValuePair<string,object>> Members

//        public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, object>> GetEnumerator()
//        {
//            throw new System.NotImplementedException();
//        }

//        #endregion

//        #region IEnumerable Members

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            throw new System.NotImplementedException();
//        }

//        #endregion

//        #region ICollection<KeyValuePair<string,object>> Members

//        public void Add(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> items)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void Remove(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> items)
//        {
//            throw new System.NotImplementedException();
//        }

//        #endregion

//        #region ICloneable Members

//        public object Clone()
//        {
//            throw new System.NotImplementedException();
//        }

//        #endregion

//        #region ISorted Members

//        public bool IsSorted
//        {
//            get { throw new System.NotImplementedException(); }
//        }

//        #endregion
//    }
//}

