using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
    public enum SelectionMode
    {
        All = 1,
        First = 2,
        Last = 3
    }

    //public enum ManipulationMode
    //{
    //    All = 1,
    //    First = 2,
    //    Last = 3
    //}

    public interface IListView<T> : ICollection<T>, System.Collections.Generic.IReadOnlyList<T>//, IStructuralComparable<T> removed - Equality comparison within an object is just wrong - comparers should be the way to go.
    {
        IEnumerable<T> this[int index, int count] { get; }

        void CopyTo(T[] array, int arrayIndex = 0, int listIndex = 0, int? count = null);

        int IndexOf(T item, SelectionMode mode = SelectionMode.First);
        
        bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First);

        // method is unnecessary -> removed
        //bool Contains(T item, SelectionMode mode = SelectionMode.First);            

        bool TryGetItem(int index, out T item);
    }
}
