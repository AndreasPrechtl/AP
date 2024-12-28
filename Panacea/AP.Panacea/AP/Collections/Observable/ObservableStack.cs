using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections.ObjectModel;
using AP.ComponentModel.Observable;
using System.Runtime.CompilerServices;

namespace AP.Collections.Observable
{
    public class ObservableStack<T> : ExtendableStack<T>, INotifyStackChanged<T>, INotifyStackChanging<T>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private const string CountString = "Count";
        private readonly List<T> _list;

        public ObservableStack()
            : base()
        {
            _list = new List<T>();
        }

        public ObservableStack(int capacity)
            : base(capacity)
        {
            _list = new List<T>(capacity);
        }
                
        public ObservableStack(int capacity, IEqualityComparer<T> comparer)
            : base(capacity, comparer)
        {
            _list = new List<T>(capacity, comparer);
        }
        
        public ObservableStack(IEqualityComparer<T> comparer)
            : base(comparer)
        {
            _list = new List<T>(comparer);
        }
        
        public ObservableStack(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : base(collection, comparer)
        {
            _list = new List<T>(collection, comparer);
        }

        public ObservableStack(IEnumerable<T> collection)
            : base(collection)
        {
            _list = new List<T>(collection);
        }
        
        protected ObservableStack(Stack<T> inner)
            : base(inner)
        {
            _list = new List<T>(inner);
        }

        public new ObservableStack<T> Clone()
        {
            return (ObservableStack<T>)this.OnClone();
        }

        protected override CollectionBase<T> OnClone()
        {
            return new ObservableStack<T>(this, this.Comparer);
        }

        public sealed override T Pop()
        {            
            return this.Pop(1).First();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<T> Pop(int count)
        {
            var list = new List<T>(_list[_list.Count - count, count]);

            if (this.OnChanging(StackChangingEventArgs<T>.Pop(this, list)))
                return null;

            var popped = this.Inner.Pop(count);

            _list.Remove(_list.Count - count, count);            
            this.OnChanged(StackChangedEventArgs<T>.Pop(this, list));
            
            return popped;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Push(T item)
        {
            var list = new List<T> { item };

            if (this.OnChanging(StackChangingEventArgs<T>.Push(this, list)))
                return;

            base.Inner.Push(item);
            this.OnChanged(StackChangedEventArgs<T>.Push(this, list));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Push(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            var list = new List<T>(items);

            if (list.Count < 1)
                return;

            if (this.OnChanging(StackChangingEventArgs<T>.Push(this, list)))
                return;

            foreach (T item in items)
            {
                list.Add(item);
                base.Inner.Push(item);
            }
            this.OnChanged(StackChangedEventArgs<T>.Push(this, list));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Clear()
        {
            if (this.Count < 1)
                return;

            var list = new List<T>(this);

            if (this.OnChanging(StackChangingEventArgs<T>.Clear(this, list)))
                return;

            base.Clear();
            this.OnChanged(StackChangedEventArgs<T>.Clear(this, list));
        }

        protected virtual void OnChanged(StackChangedEventArgs<T> e)
        {
            var changed = this.Changed;

            if (changed != null)
                changed(this, e);

            var propertyChanged = this.PropertyChanged;

            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(CountString));

            NotifyCollectionChangedEventHandler handler = this.CollectionChanged;

            if (handler != null)
                handler(this, (NotifyCollectionChangedEventArgs)e);
        }
        
        protected bool OnChanging(StackChangingEventArgs<T> e)
        {
            bool cancel;
            this.OnChanging(e, out cancel);

            return cancel;
        }

        protected virtual void OnChanging(StackChangingEventArgs<T> e, out bool cancel)
        {
            var changing = this.Changing;
            var propertyChanging = this.PropertyChanging;

            if (propertyChanging != null)
                propertyChanging(this, new PropertyChangingEventArgs(CountString));

            if (changing != null)
                changing(this, e);

            cancel = e.Cancel;
        }

        #region INotifyStackChanging<T> Members

        public event StackChangingEventHandler<T> Changing;

        #endregion

        #region INotifyCollectionChanging<T> Members

        event CollectionChangingEventHandler<T> INotifyCollectionChanging<T>.Changing
        {
            add { this.Changing += (StackChangingEventHandler<T>)(object)value; }
            remove { this.Changing -= (StackChangingEventHandler<T>)(object)value; }
        }

        #endregion

        #region INotifyStackChanged<T> Members

        public event StackChangedEventHandler<T> Changed;

        #endregion

        #region INotifyCollectionChanged<T> Members

        event CollectionChangedEventHandler<T> INotifyCollectionChanged<T>.Changed
        {
            add { this.Changed += (StackChangedEventHandler<T>)(object)value; }
            remove { this.Changed -= (StackChangedEventHandler<T>)(object)value; }
        }

        #endregion

        #region INotifyCollectionChanged Members

        protected event NotifyCollectionChangedEventHandler CollectionChanged;

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { this.CollectionChanged += value; }
            remove { this.CollectionChanged -= value; }
        }

        #endregion

        #region INotifyPropertyChanging Members

        protected event PropertyChangingEventHandler PropertyChanging;

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { this.PropertyChanging += value; }
            remove { this.PropertyChanging -= value; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        protected event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }

        #endregion
    }
}