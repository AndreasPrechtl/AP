using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace AP.Web
{
    public class Cache : IEnumerable
    {
        public static readonly DateTime NoAbsoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration;
        public static readonly TimeSpan NoSlidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
        
        private readonly System.Web.Caching.Cache _inner;
        public System.Web.Caching.Cache Inner { get {return _inner; } }
        public static AP.Web.Cache Current { get { return HttpContext.Current.Cache; } }
   
        public Cache(System.Web.Caching.Cache cache)
        {
            _inner = cache;
        }

        public static implicit operator System.Web.Caching.Cache(AP.Web.Cache cache)
        {
            return cache._inner;
        }        
        public static implicit operator AP.Web.Cache(System.Web.Caching.Cache cache)
        {
            return new AP.Web.Cache(cache);
        }

        
        public object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return _inner.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }
        
        public object Get(string key)
        {
            return _inner.Get(key);
        }
        
        public IDictionaryEnumerator GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
        
        public void Insert(string key, object value)
        {
            _inner.Insert(key, value);
        }
        
        public void Insert(string key, object value, CacheDependency dependencies)
        {
            _inner.Insert(key, value, dependencies);
        }
        
        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            _inner.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }
        
        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
            _inner.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, onUpdateCallback);
        }
        
        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            _inner.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }
        
        public object Remove(string key)
        {
            return _inner.Remove(key);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_inner).GetEnumerator();
        }
        
        public int Count
        {
            get
            {
                return _inner.Count;
            }
        }
        
        public long EffectivePercentagePhysicalMemoryLimit
        {
            get
            {
                return _inner.EffectivePercentagePhysicalMemoryLimit;
            }
        }
        
        public long EffectivePrivateBytesLimit
        {
            get
            {
                return _inner.EffectivePrivateBytesLimit;
            }
        }
        
        public object this[string key]
        {            
            get
            {
                return _inner[key];
            }            
            set
            {
                _inner[key] = value;
            }
        }        
    }
}
