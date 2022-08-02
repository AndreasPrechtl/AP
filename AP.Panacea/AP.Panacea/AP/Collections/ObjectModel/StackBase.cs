using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.ObjectModel
{
    public abstract class StackBase<T> : CollectionBase<T>, IStack<T>, IStack
    {
        #region IStack<T> Members

        public abstract void Push(T item);
        public virtual void Push(IEnumerable<T> items)
        {
            foreach (var item in items)
                this.Push(item);
        }

        public abstract T Pop();
        public virtual IEnumerable<T> Pop(int count)
        {
            for (int i = 0; i < count; ++i)
                yield return this.Pop();
        }

        public abstract T Peek();
        public abstract void Clear();

        #endregion

        #region ICloneable Members

        public new StackBase<T> Clone()
        {
            return (StackBase<T>)this.OnClone();
        }

        #endregion

        #region IStack Members

        object IStack.Pop()
        {
            return this.Pop();
        }

        IEnumerable IStack.Pop(int count)
        {
            return this.Pop(count);
        }

        void IStack.Push(object item)
        {
            this.Push((T)item);
        }

        void IStack.Push(IEnumerable items)
        {
            this.Push((IEnumerable<T>)items);
        }

        object IStack.Peek()
        {
            return this.Peek();
        }

        #endregion
    }
}
