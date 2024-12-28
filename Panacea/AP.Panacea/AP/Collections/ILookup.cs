using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
    public interface IGrouping
    { }

    public interface ILookup : IEnumerable<IGrouping>
    { }

    public interface ILookup<TKey, TValue> : System.Linq.ILookup<TKey, TValue>, ICollection<IGrouping<TKey, TValue>>
    {
        
    }
}
