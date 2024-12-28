using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.ObjectManagement
{
    public partial class ObjectManager
    {
        private class Group : IEnumerable<Item>
        {
            private readonly Dictionary<object, Item> _map = new Dictionary<object, Item>();
            private Item _default;
            private readonly object SyncRoot = new object();

            public void Register(IObjectLifetimeInternal lifetime, bool disposeOnRelease = true)
            {
                object key = lifetime.Key;

                Item item = new Item(lifetime, disposeOnRelease);

                lock (SyncRoot)
                {
                    if (key == null)
                    {
                        if (_default != null /*&& _default.Key != null */)
                            throw new InvalidOperationException("default exists");

                        _default = item;
                    }
                    else
                    {
                        //if (_default != null)
                        //    _default = item;

                        _map.Add(key, item);
                    }
                }
            }

            public bool Contains(object key = null)
            {
                if (key == null)
                    return _default != null;

                // check if a default exists or not... 
                return _map.ContainsKey(key); // || _default != null;                    
            }

            public IObjectLifetimeInternal Get(object key = null)
            {
                if (key == null)
                    return _default.Lifetime;

                // return a default if theres no keyed value?
                return _map[key].Lifetime;
                
                //Item lt = null;
                //if (_map.TryGetValue(key, out lt))
                //    return lt.Lifetime;
                                
                //return _default.Lifetime;
            }

            public void Release(object key = null)
            {
                Item item = null;

                if (key == null)
                {
                    item = _default;
                    _default = null;
                }
                else
                {
                    item = _map[key];
                    _map.Remove(key);
                }

                item.OnReleased();
            }

            public void Clear()
            {
                _default.OnReleased();

                foreach (var kvp in _map)
                    kvp.Value.OnReleased();
                
                _map.Clear();
            }

            public bool IsEmpty { get { return _default == null && _map.Count == 0; } }

            public bool TryGetValue(out IObjectLifetimeInternal lifetime, object key = null)
            {
                lifetime = null;
                bool r = false;

                if (key != null)
                {
                    Item lt = null;
                    if (r = _map.TryGetValue(key, out lt))
                        lifetime = lt.Lifetime;
                }
                else if (r = (_default != null))
                    lifetime = _default.Lifetime;                    

                return r;
            }

            #region IEnumerable<Item> Members

            public IEnumerator<Item> GetEnumerator()
            {
                return _map.Values.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion
        }
    }
}
