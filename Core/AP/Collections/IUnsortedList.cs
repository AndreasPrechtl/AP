using System.Collections.Generic;

namespace AP.Collections
{
    public interface IUnsortedList<T> : AP.Collections.IList<T>, System.Collections.Generic.IList<T>
    {   
        new void Insert(int index, T item);
        void Insert(int index, IEnumerable<T> items);
        
        void Move(int index, int newIndex, int count = 1);

        void Replace(int index, IEnumerable<T> items);

        new T this[int index] { get; set; }
        
        new void Clear();
    }
}
