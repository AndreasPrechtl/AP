using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.ObjectModel
{
    public abstract class QueueBase<T> : CollectionBase<T>, IQueue<T>
    {
        #region IQueue<T> Members

        public abstract void Enqueue(T item);
        public virtual void Enqueue(IEnumerable<T> items)
        {
            foreach (var item in items)
                this.Enqueue(item);
        }

        public abstract T Dequeue();
        public virtual IEnumerable<T> Dequeue(int count)
        {
            for (int i = 0; i < count; ++i)
                yield return this.Dequeue();
        }

        public abstract T Peek();
        public abstract void Clear();

        #endregion
                
        #region ICloneable Members

        public new QueueBase<T> Clone()
        {
            return (QueueBase<T>)this.OnClone();
        }

        #endregion
    }
}
