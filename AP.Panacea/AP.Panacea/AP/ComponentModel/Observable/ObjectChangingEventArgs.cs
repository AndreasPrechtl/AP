using System;

namespace AP.ComponentModel.Observable
{
    public class ObjectChangingEventArgs : System.ComponentModel.CancelEventArgs
    {
        private readonly object _oldValue;
        private readonly object _newValue;
        private readonly object _container;

        public object Container { get { return _container; } }
        public object OldValue { get { return _oldValue; } }
        public object NewValue { get { return _newValue; } }

        public ObjectChangingEventArgs(object container, object oldValue, object newValue)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public virtual bool HasChanges
        {
            get { return object.Equals(_oldValue, _newValue); }
        }
    }

    public class ObjectChangingEventArgs<T> : ObjectChangingEventArgs        
    {
        public new T Container { get { return (T)base.Container; } }

        public ObjectChangingEventArgs(T container, object oldValue, object newValue)
            : base(container, oldValue, newValue)
        { }
    }
    
    public class ObjectChangingEventArgs<T, TValue> : ObjectChangingEventArgs<T>
    {
        public new TValue OldValue { get { return (TValue)base.OldValue; } }
        public new TValue NewValue { get { return (TValue)base.NewValue; } }
                
        public ObjectChangingEventArgs(T container, TValue oldValue, TValue newValue)
            : base(container, oldValue, newValue)
        { }
    }
}