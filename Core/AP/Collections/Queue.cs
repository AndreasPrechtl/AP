using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using System.Collections;
using SCG = System.Collections.Generic;

namespace AP.Collections
{
    public class Queue<T> : IQueue<T>, IEqualityComparerUser<T>
    {
        private readonly System.Collections.Generic.Queue<T> _inner;
        private readonly IEqualityComparer<T> _comparer;

        public Queue()
            : this(0)
        { }

        public Queue(IEqualityComparer<T> comparer)
            : this(0, comparer)
        { }

        public Queue(int capacity)
            : this(capacity, null)
        { }
        
        public Queue(int capacity, IEqualityComparer<T> comparer)
            : this(new SCG.Queue<T>(capacity), comparer ?? EqualityComparer<T>.Default)
        { }

        public Queue(IEnumerable<T> collection)
            : this(collection, null)
        { }

        public Queue(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(collection == null ? null : new SCG.Queue<T>(collection), comparer ?? EqualityComparer<T>.Default)
        { }

        protected Queue(SCG.Queue<T> inner, IEqualityComparer<T> comparer)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _inner = inner ?? new SCG.Queue<T>(0);
            _comparer = comparer;
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public static Queue<T> Wrap(SCG.Queue<T> queue, IEqualityComparer<T> comparer = null)
        {
            return new Queue<T>(queue, comparer ?? EqualityComparer<T>.Default);
        }

        public static implicit operator AP.Collections.Queue<T>(SCG.Queue<T> queue)
        {
            return new AP.Collections.Queue<T>(queue, EqualityComparer<T>.Default);
        }

        public static implicit operator SCG.Queue<T>(AP.Collections.Queue<T> queue)
        {
            return queue._inner;
        }

        #region IQueue<T> Members

        public T Dequeue()
        {
            return _inner.Dequeue();
        }

        public IEnumerable<T> Dequeue(int count)
        {
            for (int i = 0; i < count; ++i)
                yield return _inner.Dequeue();
        }

        public void Enqueue(T item)
        {
            _inner.Enqueue(item);
        }

        public void Enqueue(IEnumerable<T> items)
        {
            foreach (T item in items)
                _inner.Enqueue(item);
        }

        public T Peek()
        {
            return _inner.Peek();
        }

        public void Clear()
        {
            _inner.Clear();
        }

        #endregion

        #region ICollection<T> Members

        public int Count
        {
            get { return _inner.Count; }
        }

        public void CopyTo(T[] array, int arrayIndex = 0)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public bool Contains(T item)
        {
            foreach (T current in _inner)
                if (_comparer.Equals(item, current))
                    return true;

            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion
               
        #region ICloneable Members

        public Queue<T> Clone()
        {
            return this.OnClone();
        }

        protected virtual Queue<T> OnClone()
        {
            return new Queue<T>(this, _comparer);
        }

        object ICloneable.Clone()
        {
            return this.OnClone();
        }

        #endregion

        #region IEqualityComparerUser<T> Members

        public IEqualityComparer<T> Comparer
        {
            get { return _comparer; }
        }

        #endregion
    }
}
