using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.ObjectManagement
{
    internal interface IManagedInstanceInternal : System.IDisposable
    {
        object Value { get; }
        bool IsDisposed { get; }
    }

    public sealed class ManagedInstance<TBase> : System.IDisposable, IWrapper<TBase>, IManagedInstanceInternal
    {
        private TBase _value;
        private readonly bool _canDisposeInstance;
        private bool _isDisposed;
        private Action _callback;

        /// <summary>
        /// Creates a new ManagedInstance.
        /// </summary>
        /// <param name="instance">The instance to manage.</param>
        /// <param name="canDisposeInstance">When true, the instance will disposed alongside.</param>
        public ManagedInstance(TBase instance, bool canDisposeInstance = false, Action disposedCallback = null)            
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            _value = instance;
            _canDisposeInstance = canDisposeInstance;
            _callback = disposedCallback;

            if (instance is AP.IDisposable)
                ((AP.IDisposable)instance).Disposing += this.OnValueDisposing;   
        }

        public static ManagedInstance<TBase> Create(TBase instance, bool canDisposeInstance = false)
        {
            return new ManagedInstance<TBase>(instance, canDisposeInstance);
        }

        public bool IsDisposed 
        { 
            get { return _isDisposed; } 
        }

        public bool CanDisposeInstance
        {
            get { return _canDisposeInstance; }
        }

        private void OnValueDisposing(object sender, EventArgs e)
        {
            TBase value = _value;

            if (value != null)
                ((AP.IDisposable)value).Disposing -= this.OnValueDisposing;
            
            this.Dispose();
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Object is already disposed", (Exception)null);
        }

        #region IWrapper<TBase> Members

        /// <summary>
        /// Gets the actual instance.
        /// </summary>
        public TBase Value
        {
            get
            {
                this.ThrowIfDisposed();
                return _value; 
            }
        }

        #endregion

        public void Dispose()
        {
            TBase value = _value;

            if (_isDisposed)
                return;

            if (_canDisposeInstance && value != null)
            {
                if (value is AP.IDisposable)
                    ((AP.IDisposable)value).Disposing -= this.OnValueDisposing;

                value.TryDispose();
            }

            _isDisposed = true;
            
            Action callback = _callback;

            if (callback != null)
                callback();
            
            _value = default(TBase);
        }

        public override bool Equals(object obj)
        {
            this.ThrowIfDisposed();

            if (obj == null)
                return false;

            if (this == obj)
                return true;

            if (obj is IManagedInstanceInternal)
                return _value.Equals(((IManagedInstanceInternal)obj).Value);

            return _value.Equals(obj);
        }

        public override int GetHashCode()
        {
            this.ThrowIfDisposed();
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            this.ThrowIfDisposed();
            return _value.ToString();
        }

        public static implicit operator TBase(ManagedInstance<TBase> instance)
        {
            return instance.Value;
        }

        #region IManagedInstanceInternal Members

        object IManagedInstanceInternal.Value
        {
            get { return _value; }
        }

        #endregion
    }
}
