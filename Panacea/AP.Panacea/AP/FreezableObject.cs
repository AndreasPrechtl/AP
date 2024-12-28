using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AP
{
    /// <summary>
    /// Base class for freezable objects
    /// </summary>
    public abstract class FreezableObject : IFreezable
    {
        private bool _isReadOnly;

        /// <summary>
        /// Creates a new FreezableObject
        /// </summary>
        /// <param name="isReadOnly">Indicates if the object should be frozen directly</param>
        protected FreezableObject(bool isReadOnly = false)
        {
            _isReadOnly = isReadOnly;
        }

        #region IFreezable Members

        /// <summary>
        /// Gets the current status or freezes the object;
        /// once it is frozen the setter no longer works
        /// </summary>
        /// <exception cref="System.InvalidOperationException" />
        public virtual bool IsFrozen
        {
            get { return _isReadOnly; }
            set
            {
                this.AssertCanWrite();
                _isReadOnly = value;
            }
        }

        /// <summary>
        /// Throws a System.InvalidOperationException if the object is frozen and therefore readonly
        /// </summary>        
        protected virtual void AssertCanWrite()
        {
            FreezableHelper.AssertCanWrite(this);
        }

        #endregion
    }
}
