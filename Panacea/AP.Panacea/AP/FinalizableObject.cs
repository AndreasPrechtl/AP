using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AP
{
    /// <summary>
    /// A DisposableObject base class with a finalizer
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "IDisposable is implemented correctly - yet simplified for less complexity on child classes")]
    public abstract class FinalizableObject : DisposableObject
    {
        /// <summary>
        /// Creates a new inherited FinalizableObject
        /// </summary>
        /// <param name="contextKey">The key that can be used to safely dispose the object</param>
        protected FinalizableObject(object contextKey = null)
            : base(contextKey)
        { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        ~FinalizableObject()
        {
            this.Dispose(this.ContextKey);
        }

        internal sealed override void SuppressFinalizeIfNeeded()
        {
            GC.SuppressFinalize(this);
        }
    }
}