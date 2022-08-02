using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AP.ComponentModel;

namespace AP.Data
{
    /// <summary>
    /// Baseclass for EntityContexts with an external DataProvider.
    /// </summary>
    /// <typeparam name="TDataProvider">The DataProvider.</typeparam>
    public abstract class EntityContextBase<TDataProvider> : EntityContextBase
        where TDataProvider : class
    {
        private TDataProvider _inner;
        private readonly bool _canDisposeContext;

        /// <summary>
        /// Gets the DataProvider.
        /// </summary>
        public TDataProvider Provider
        {
            get 
            { 
                this.ThrowIfDisposed();
                return _inner;
            }            
        }

        protected EntityContextBase(TDataProvider dataContext, bool ownsProvider = true, SaveMode saveMode = Data.SaveMode.Default, object contextKey = null)
            : base(saveMode, contextKey)
        {
            ExceptionHelper.AssertNotNull(() => dataContext);

            _inner = dataContext;
            _canDisposeContext = ownsProvider;

            if (!ownsProvider && dataContext is AP.IDisposable)
            {
                ((AP.IDisposable)dataContext).Disposed += DataContextDisposed;
            }
        }

        private void DataContextDisposed(object sender, EventArgs e)
        {
            if (_inner != null)
            {
                ((AP.IDisposable)_inner).Disposing -= this.DataContextDisposed;
                this.CleanUpResources();
            }
        }
        
        protected override void CleanUpResources()
        {
            base.CleanUpResources();
         
            if (_canDisposeContext && _inner != null)
                _inner.TryDispose();

            _inner = null;
        }
    }
}
