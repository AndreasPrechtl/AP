using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Linq;

namespace AP.ComponentModel
{
    public class DynamicProxy<T> : DynamicObject, IWrapper<T>
    {
        private readonly T _inner;
        private readonly ExpandoObject _expando;

        public DynamicProxy(T obj)
        {
            ExceptionHelper.ThrowOnArgumentNullException(() => obj);
            _inner = obj;
            _expando = new ExpandoObject();
        }

        #region IWrapper<T> Members

        public T Value
        {
            get { return _inner; }
        }

        #endregion

        public sealed override IEnumerable<string> GetDynamicMemberNames()
        {
            return ((IDictionary<string, object>)_expando).Keys;
        }
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            return ((IDictionary<string,object>)_expando).Remove(binder.Name);
        }
        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            return false;
        }
    }
}
