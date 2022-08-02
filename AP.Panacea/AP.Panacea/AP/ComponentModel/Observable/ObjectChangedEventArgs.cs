using System;

namespace AP.ComponentModel.Observable
{
    public class ObjectChangedEventArgs : EventArgs
    {
        private readonly object _oldValue;
        private readonly object _newValue;
        private readonly object _container;

        public object Container { get { return _container; } }
        public object OldValue { get { return _oldValue; } }
        public object NewValue { get { return _newValue; } }

        public ObjectChangedEventArgs(object container, object oldValue, object newValue)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
            _oldValue = oldValue;
            _newValue = newValue;            
        }

        public bool HasChanges
        {
            get { return object.Equals(_oldValue, _newValue); }
        }
    }

    public class ObjectChangedEventArgs<T> : ObjectChangedEventArgs
    {
        public new T Container { get { return (T)base.Container; } }

        public ObjectChangedEventArgs(T container, object oldValue, object newValue)
            : base(container, oldValue, newValue)
        { }
    }

    public class ObjectChangedEventArgs<T, TValue> : ObjectChangedEventArgs<T>
    {
        public new TValue OldValue { get { return (TValue)base.OldValue; } }
        public new TValue NewValue { get { return (TValue)base.NewValue; } }

        public ObjectChangedEventArgs(T container, TValue oldValue, TValue newValue)
            : base(container, oldValue, newValue)
        { }
    }
}