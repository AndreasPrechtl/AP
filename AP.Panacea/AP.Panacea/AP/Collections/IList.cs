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

    public interface IListView : ICollection
    {
        IEnumerable this[int index, int count] { get; }

        void CopyTo(Array array, int arrayIndex = 0, int listIndex = 0, int? count = null);

        int IndexOf(object item, SelectionMode mode = SelectionMode.First);

        bool Contains(object item, out int index, SelectionMode mode = SelectionMode.First);

        // method is unnecessary -> removed
        //bool Contains(object item, SelectionMode mode = SelectionMode.First);

        bool TryGetItem(int index, out object item);
    }

    public interface IList<T> : IListView<T>, System.Collections.Generic.ICollection<T>
    {
        new int Add(T item);
        void Add(IEnumerable<T> items);
        void Remove(int index, int count = 1);
        void Remove(T item, SelectionMode mode = SelectionMode.First);
        new void Clear();
    }

    public interface IList : IListView
    {
        int Add(object item);
        void Add(IEnumerable items);
        void Remove(int index, int count = 1);
        void Remove(object item, SelectionMode mode = SelectionMode.First);
        void Clear();
    }
    
    public interface IUnsortedList<T> : IList<T>, System.Collections.Generic.IList<T>
    {   
        new void Insert(int index, T item);
        void Insert(int index, IEnumerable<T> items);
        
        void Move(int index, int newIndex, int count = 1);

        void Replace(int index, IEnumerable<T> items);

        new T this[int index] { get; set; }
        
        new void Clear();
    }
    
    public interface IUnsortedList : IList, System.Collections.IList
    {
        new void Insert(int index, object item);
        void Insert(int index, IEnumerable items);

        void Move(int index, int newIndex, int count = 1);

        void Replace(int index, IEnumerable items);

        new object this[int index] { get; set; }

        new void Clear();
    }
}
