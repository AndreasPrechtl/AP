using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

using AP;
using AP.Linq;
using AP.ComponentModel;
using AP.ComponentModel.ObjectManagement;

namespace AP.Web.objectManagement
{
    /// <summary>
    /// Shouldn't be used until I find a way to release it from cache without having to rely on the finalizer
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    public class CacheLifetime<TBase> : ObjectLifetimeBase<TBase>
    {
        private readonly string _cacheKey = Guid.NewGuid().ToString();
        protected string CacheKey { get { return _cacheKey; } }
        private readonly DateTime _expirationDate;
        private readonly TimeSpan _slidingExpirationTime;
        private readonly CacheItemPriority _priority;
        private Activator<TBase> _activator;

        public CacheLifetime(Activator<TBase> activator, object key = null, DateTime? expirationDate = null, TimeSpan? slidingExpirationTime = null, CacheItemPriority priority = CacheItemPriority.Default)
            : base(key)
        {
            if (activator == null)
                throw new ArgumentNullException("activator");

            _activator = activator;
            _expirationDate = expirationDate ?? Cache.NoAbsoluteExpiration;
            _slidingExpirationTime = slidingExpirationTime ?? Cache.NoSlidingExpiration;
            _priority = priority;
        }

        public CacheLifetime(TBase instance, object key = null, DateTime? expirationDate = null, TimeSpan? slidingExpirationTime = null, CacheItemPriority priority = CacheItemPriority.Default)
            : this(() => instance, key, expirationDate, slidingExpirationTime, priority)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            HttpContext.Current.Cache.Add(_cacheKey, instance, null, expirationDate ?? Cache.NoAbsoluteExpiration, slidingExpirationTime ?? Cache.NoSlidingExpiration, _priority, this.HandleCachedInstanceRemoved);
            ApplicationBase.Instance.ApplicationEnded += HandleApplicationEnded;
        }

        private void HandleApplicationEnded(object sender, ApplicationEventArgs e)
        {
            HttpContext context = e.HttpContext;

            ApplicationBase.Instance.ApplicationEnded -= HandleApplicationEnded;
            Cache cache = context.Cache;

            object instance = cache[_cacheKey];

            if (instance != null)
                cache.Remove(_cacheKey);

            instance.TryDispose();
        }

        public DateTime ExpirationDate { get { return _expirationDate; } }
        public TimeSpan SlidingExpirationTime { get { return _slidingExpirationTime; } }
        public CacheItemPriority Priority { get { return _priority; } }

        public override ManagedInstance<TBase> Instance
        {
            get
            {
                object instance = HttpContext.Current.Cache[_cacheKey];

                // just leave it as it is - if the cache is cleared on the way there - it's no problem re-creating the instance
                if (instance == null)
                {
                    instance = _activator();
                    HttpContext.Current.Cache.Add(_cacheKey, instance, null, this.ExpirationDate, this.SlidingExpirationTime, _priority, this.HandleCachedInstanceRemoved);
                }
                return new ManagedInstance<TBase>((TBase)instance, false);
            }
        }

        private void HandleCachedInstanceRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            // simple cleansweap - don't need to remove it from within the cache - it should already be removed afterwards
            if (key.Equals(_cacheKey))
                value.TryDispose();
        }
        
        protected override void CleanUpResources()
        {
            base.CleanUpResources();

            _activator = null;

            HttpContext context = HttpContext.Current;

            if (context == null)
                return;

            Cache cache = HttpContext.Current.Cache;            
            object instance = cache[_cacheKey];

            // disposal is handled within the HandleCachedInstanceRemoved callback method
            if (instance != null)
                cache.Remove(_cacheKey);

            base.CleanUpResources();
        }
    }
}
