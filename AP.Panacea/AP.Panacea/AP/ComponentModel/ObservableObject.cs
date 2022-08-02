using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AP;
using AP.Reflection;
using System.Reflection;

namespace AP.ComponentModel
{
    public abstract class ObservableObject : IObservable
    {
        private readonly Type _type;

        protected ObservableObject()
        {
            _type = this.GetType();
        }

        /// <summary>
        /// Very simple shorthand method for !object.Equals(v1, v2) - inlined aggressively
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsValueChanging<TValue>(TValue currentValue, TValue newValue)
        {
            return !object.Equals(currentValue, newValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]        
        private static void AssertPropertyExists(ObservableObject current, string propertyName)
        {
            ExceptionHelper.ThrowOnArgumentNullException(() => propertyName);
            PropertyInfo propertyInfo = current._type.GetProperty(propertyName);

            if (propertyInfo == null)
                ExceptionHelper.ThrowArgumentOutOfRangeException(() => propertyName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]        
        private static void AssertMethodExists(ObservableObject current, string methodName)
        {
            ExceptionHelper.ThrowOnArgumentNullException(() => methodName);
            MethodInfo methodInfo = current._type.GetMethod(methodName);

            if (methodInfo == null)
                ExceptionHelper.ThrowArgumentOutOfRangeException(() => methodName);
        }

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]        
        protected virtual void OnPropertyChanging<TValue>(TValue currentValue, TValue newValue, [CallerMemberName] string propertyName = null)
        {
            AssertPropertyExists(this, propertyName);
            this.PropertyChanging.Raise(this, new PropertyChangingEventArgs(propertyName, currentValue, newValue));            
        }
        
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]        
        protected virtual void OnPropertyChanged<TValue>(TValue newValue, TValue oldValue, [CallerMemberName] string propertyName = null)
        {
            AssertPropertyExists(this, propertyName);
            this.PropertyChanged.Raise(this, new PropertyChangedEventArgs(propertyName, newValue, oldValue));
        }

        protected void SetValue<TValue>(ref TValue currentValue, TValue newValue, [CallerMemberName] string propertyName = null)
        {
            if (!this.IsValueChanging(currentValue, newValue))
                return;

            this.OnPropertyChanging(currentValue, newValue, propertyName);
            
            TValue oldValue = currentValue;
            currentValue = newValue;

            this.OnPropertyChanged(newValue, oldValue, propertyName);
        }
        
        #endregion

        #region IObservable Members

        public event MethodCalledEventHandler MethodCalled;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnMethodCalled([CallerMemberName] string methodName = null)
        {
            AssertMethodExists(this, methodName);
            this.MethodCalled.Raise(this, new MethodCalledEventArgs(methodName));
        }

        #endregion

        #region INotifyPropertyChanging Members

        event System.ComponentModel.PropertyChangingEventHandler System.ComponentModel.INotifyPropertyChanging.PropertyChanging
        {
            add { this.PropertyChanging += new PropertyChangingEventHandler(value); }
            remove { this.PropertyChanging -= new PropertyChangingEventHandler(value); }
        }

        #endregion

        #region INotifyPropertyChanged Members

        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        #endregion
    }
}
