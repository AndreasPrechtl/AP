using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AP.Collections
{
    [System.ComponentModel.ReadOnly(true)]
    public sealed class ReadOnlyEnumerable : IEnumerable
    {
        private readonly IEnumerable _source;

        public ReadOnlyEnumerable(IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _source = source;
        }

        #region IEnumerable Members

        // [MethodImpl((MethodImplOptions)256)]
        public IEnumerator GetEnumerator()
        {
            foreach (object item in _source)
                yield return item;
        }

        #endregion      
    }

    [System.ComponentModel.ReadOnly(true)]
    public sealed class ReadOnlyEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        
        public ReadOnlyEnumerable(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _source = source;
        }

        #region IEnumerable<T> Members

        // [MethodImpl((MethodImplOptions)256)]
        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();

            // use this for a completely clean enumerator:
            //foreach (T item in _source)
            //    yield return item;            
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
