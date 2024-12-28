using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AP;

namespace AP.ComponentModel
{
    public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {   
        /// <summary>
        /// Very simple shorthand method for !object.Equals(v1, v2) - inlined aggressively
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        
        protected bool IsValueChanging<T>(T oldValue, T newValue)
        {
            return !object.Equals(oldValue, newValue);
        }
        
        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

                
        protected void OnPropertyChanging(string propertyName = null)
        {            
            this.PropertyChanging.Raise(this, new PropertyChangingEventArgs(propertyName));            
        }
        
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

                
        protected void OnPropertyChanged(string propertyName = null)
        {   
            this.PropertyChanged.Raise(this, new PropertyChangedEventArgs(propertyName));
        }
        
        #endregion
    }
}
