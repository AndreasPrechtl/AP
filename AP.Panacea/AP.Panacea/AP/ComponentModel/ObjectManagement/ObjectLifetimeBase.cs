using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP;
using AP.ComponentModel;
using System.Reflection;
using AP.Reflection;
using System.Linq.Expressions;
using System.Reflection.Emit;
using AP.Linq;

namespace AP.ComponentModel.ObjectManagement
{
    public abstract class ObjectLifetimeBase<TBase> : DisposableObject, IObjectLifetimeInternal
    {
        private readonly object _key;

        protected ObjectLifetimeBase(object key = null)
        {
            _key = key;
        }

        public abstract ManagedInstance<TBase> Instance { get; }

        #region IObjectLifetimeInternal Members
                
        public object Key
        {
            get
            {
                return _key;
            }           
        }
        
        //object IObjectLifetimeInternal.Instance
        //{
        //    get { return this.Instance; }
        //}

        #endregion
    }
}
