using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AP
{
    public abstract class PropertiesHelper : StaticType
    {
        [MethodImpl((MethodImplOptions)256)]
        public static T GetValueDeferred<T>(ref T referencedValue, Func<T> resolver, bool setValue = true, object syncRoot = null)
        {
            T tmp = referencedValue;
            if (!object.Equals(tmp, default(T)))
            {
                tmp = resolver();

                if (setValue)
                {
                    if (syncRoot == null)
                        referencedValue = tmp;
                    else
                    {
                        lock (syncRoot)
                            referencedValue = tmp;
                    }
                }
            }
            return tmp;
        }
    }
}
