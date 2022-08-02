using AP.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.Freezable
{
    [Serializable]
    public class FreezableStack<T> : ExtendableStack<T>, IFreezable
    {
        public FreezableStack()
            : base()
        {
            _isFrozen = false;
        }

        public FreezableStack(IEqualityComparer<T> comparer)
            : base(comparer)
        {
            _isFrozen = false;
        }

        public FreezableStack(int capacity)
            : base(capacity)
        {
            _isFrozen = false;
        }
        
        public FreezableStack(int capacity, IEqualityComparer<T> comparer)
            : base(capacity, comparer)
        {
            _isFrozen = false;
        }

        public FreezableStack(IEnumerable<T> collection, bool isFrozen = false)
            : base(collection)
        {
            _isFrozen = isFrozen;
        }

        public FreezableStack(IEnumerable<T> collection, IEqualityComparer<T> comparer, bool isFrozen = false)
            : base(collection, comparer)
        {
            _isFrozen = isFrozen;
        }

        protected FreezableStack(Stack<T> inner, bool isFrozen = false)
            : base(inner)
        {
            _isFrozen = isFrozen;
        }
        
        public new FreezableStack<T> Clone()
        {
            return (FreezableStack<T>)this.OnClone();
        }

        protected override CollectionBase<T> OnClone()
        {
            return new FreezableStack<T>(this, this.Comparer, this.IsFrozen);
        }

        public override void Clear()
        {
            AssertCanWrite();
            base.Clear();
        }
        
        public override T Pop()
        {
            AssertCanWrite();
            return base.Pop();
        }
        
        public override void Push(IEnumerable<T> items)
        {
            AssertCanWrite();
            base.Push(items);
        }

        public override void Push(T item)
        {
            AssertCanWrite();
            base.Push(item);
        }

        #region IFreezable Members

        private bool _isFrozen;

        public bool IsFrozen
        {
            get { return _isFrozen; }
            set
            {
                this.AssertCanWrite();
                _isFrozen = value;
            }
        }

        protected virtual void AssertCanWrite()
        {
            FreezableHelper.AssertCanWrite(this);
        }

        #endregion
    }
}
