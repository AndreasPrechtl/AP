using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.ComponentModel;

namespace AP.Data
{
    ///// <summary>
    ///// Experimental - don't use this yet.
    ///// </summary>
    //public static class EntityReference
    //{
    //    public static EntityReference<TKey, TEntity> Create<TKey, TEntity>(Func<TEntity, TKey> keySelector, Func<TKey, TEntity> valueResolver)
    //        where TEntity : class
    //    {
    //        return new EntityReference<TKey, TEntity>(keySelector, valueResolver);
    //    }

    //    //public static EntityReference<TKey, TEntity> Create<TKey, TEntity>(Func<TKey, TEntity> valueResolver)
    //    //    where TEntity : class, IKeyUser<TKey>
    //    //{
    //    //    return new EntityReference<TKey, TEntity>(p => p.Key, valueResolver);
    //    //}
    //}

    //public abstract class EntityReference<TEntity> : IEntityReference<TEntity>
    //    where TEntity : class
    //{
    //    protected TEntity _value;

    //    public virtual TEntity Value
    //    {
    //        get
    //        {
    //            return _value;
    //        }
    //        set
    //        {
    //            _value = value;
    //        }
    //    }

    //    public bool HasValue
    //    {
    //        get { return _value != null; }
    //    }
    //}

    public class EntityReference<TKey, TEntity> : IEntityReference<TKey, TEntity>
        where TEntity : class
    {
        private readonly Func<TKey, TEntity> _valueResolver;
        private readonly Func<TEntity, TKey> _keySelector;

        public EntityReference(Func<TEntity, TKey> keySelector, Func<TKey, TEntity> valueResolver)
        {
            ExceptionHelper.AssertNotNull(() => keySelector);
            ExceptionHelper.AssertNotNull(() => valueResolver);

            _keySelector = keySelector;
            _valueResolver = valueResolver;
        }

        private TEntity _value;
        private TKey _key;

        private bool _hasKey = false;

        public TKey Key
        {
            get
            {
                if (_hasKey)
                    return _key;

                TKey key = _keySelector(_value);
                
                _key = key;
                _hasKey = true;

                return key;
            }
            set
            {
                if (value.IsNull())
                    throw new ArgumentNullException("value");
                
                if (!object.Equals(_key, value))
                {   
                    _key = value;
                    _hasKey = true;
                    _value = null;
                }
            }
        }

        public TEntity Value
        {
            get
            {
                TEntity value = _value;

                if (value == null)
                    _value = value = _valueResolver(_key);
                
                return value;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
            
                if (!object.Equals(_value, value))
                {
                    _value = value;
                    _hasKey = false;
                    _key = default(TKey);
                }
            }
        }

        public bool HasValue
        {
            get
            {
                return _value != null;
            }
        }

        protected Func<TKey, TEntity> ValueResolver
        {
            get { return _valueResolver; }
        }

        protected Func<TEntity, TKey> KeySelector
        {
            get { return _keySelector; }
        }


        #region IEquatable<IEntityReference<TKey,TEntity>> Members

        public virtual bool Equals(IEntityReference<TKey, TEntity> other)
        {
            if (other == null)
                return false;

            if (other == this)
                return true;

            return this.Key.Equals(other.Key) && this.Value.Equals(other.Value);
        }

        #endregion

        #region IEquatable<TKey> Members

        public virtual bool Equals(TKey other)
        {
            return this.Key.Equals(other);
        }

        #endregion

        #region IComparable<TKey> Members

        public virtual int CompareTo(TKey key)
        {
            return Comparer<TKey>.Default.Compare(this.Key, key);
        }

        #endregion

        #region IComparable Members

        public virtual int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj == this)
                return 0;

            if (obj is IEntityReference<TKey, TEntity>)
                return this.CompareTo((IEntityReference<TKey, TEntity>)obj);

            return 1;
        }

        #endregion
    }
}
