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
    public class ObservableQueue<T> : ExtendableQueue<T>, INotifyQueueChanged<T>, INotifyQueueChanging<T>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private const string CountString = "Count";
              
        public ObservableQueue()
            : base()
        { }

        public ObservableQueue(int capacity)
            : base(capacity)
        { }
                
        public ObservableQueue(int capacity, IEqualityComparer<T> comparer)
            : base(capacity, comparer)
        { }
        
        public ObservableQueue(IEqualityComparer<T> comparer)
            : base(comparer)
        { }
        
        public ObservableQueue(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : base(collection, comparer)
        { }

        public ObservableQueue(IEnumerable<T> collection)
            : base(collection)
        { }
        
        protected ObservableQueue(Queue<T> inner)
            : base(inner)
        { }

        public new ObservableQueue<T> Clone()
        {
            return (ObservableQueue<T>)this.OnClone();
        }

        protected override CollectionBase<T> OnClone()
        {
            return new ObservableQueue<T>(this, this.Comparer);
        }

        public sealed override T Dequeue()
        {
            return this.Dequeue(1).First();
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<T> Dequeue(int count)
        {
            var list = new List<T>(this.Take(count));

            if (this.OnChanging(QueueChangingEventArgs<T>.Dequeue(this, list)))
                return null;
            
            var dequeued = base.Dequeue(count);
            this.OnChanged(QueueChangedEventArgs<T>.Dequeue(this, list));

            return dequeued;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Enqueue(T item)
        {
            var list = new List<T> { item };

            if (this.OnChanging(QueueChangingEventArgs<T>.Enqueue(this, list)))
                return;

            base.Inner.Enqueue(item);            
            this.OnChanged(QueueChangedEventArgs<T>.Enqueue(this, list));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Enqueue(IEnumerable<T> items)
        {
            if (items == null) 
                throw new ArgumentNullException("items");

            var list = new List<T>(items);

            if (list.Count < 1)
                return;
            
            if (this.OnChanging(QueueChangingEventArgs<T>.Enqueue(this, list)))
                return;
            
            foreach (T item in items)
            {
                list.Add(item);
                base.Inner.Enqueue(item);
            }

            this.OnChanged(QueueChangedEventArgs<T>.Enqueue(this, list));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Clear()
        {
            if (this.Count < 1)
                return;

            var list = new List<T>(this);
            
            var a = QueueChangingEventArgs<T>.Clear(this, list);
            this.OnChanging(a);
            if (a.Cancel)
                return;
            
 	        base.Clear();
            this.OnChanged(QueueChangedEventArgs<T>.Clear(this, list));
        }

        protected virtual void OnChanged(QueueChangedEventArgs<T> e)
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

        protected bool OnChanging(QueueChangingEventArgs<T> e)
        {
            bool cancel;
            this.OnChanging(e, out cancel);
            
            return cancel;
        }

        protected virtual void OnChanging(QueueChangingEventArgs<T> e, out bool cancel)
        {
            var changing = this.Changing;
            var propertyChanging = this.PropertyChanging;

            if (propertyChanging != null)
                propertyChanging(this, new PropertyChangingEventArgs(CountString));
            
            if (changing != null)
                changing(this, e);

            cancel = e.Cancel;
        }

        #region INotifyQueueChanging<T> Members

        public event QueueChangingEventHandler<T> Changing;

        #endregion

        #region INotifyCollectionChanging<T> Members

        event CollectionChangingEventHandler<T> INotifyCollectionChanging<T>.Changing
        {
            add { this.Changing += new QueueChangingEventHandler<T>(value); }
            remove { this.Changing -= new QueueChangingEventHandler<T>(value); }
        }

        #endregion
        
        #region INotifyQueueChanged<T> Members

        public event QueueChangedEventHandler<T> Changed;

        #endregion

        #region INotifyCollectionChanged<T> Members

        event CollectionChangedEventHandler<T> INotifyCollectionChanged<T>.Changed
        {
            add { this.Changed += new QueueChangedEventHandler<T>(value); }
            remove { this.Changed -= new QueueChangedEventHandler<T>(value); }
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
