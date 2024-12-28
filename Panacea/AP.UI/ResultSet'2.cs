using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Linq;

namespace AP.UI
{
    internal class ResultSet<T> : IViewModel<T>
    {
        private readonly T _current;

        public T Current
        {
            get
            {
                if (!this.HasCurrent)
                    throw new Exception("Current");

                return _current;
            }
        }

        private readonly int _count;

        private readonly T _first;
        private readonly T _previous;
        private readonly T _next;
        private readonly T _last;
        private readonly SortDirection _sortDirection;

        public SortDirection SortDirection
        {
            get { return _sortDirection; }
        }

        public ResultSet(T first, T previous, T current, T next, T last, int count, SortDirection sortDirection)
        {
            _count = count;
            _sortDirection = sortDirection;

            if (count > 0)
            {
                _current = current;
                _first = first.IsDefault() ? previous : first;
                _previous = previous;
                _next = next;
                _last = last.IsDefault() ? next : last;
            }
        }

        public T First
        {
            get
            {
                if (!HasFirst)
                    throw new Exception("First");

                return _first;
            }
        }
        public T Previous
        {
            get
            {
                if (!this.HasPrevious)
                    throw new Exception("Previous");

                return _previous;
            }
        }

        public T Next
        {
            get
            {
                if (!this.HasNext)
                    throw new Exception("Next");

                return _next;
            }
        }

        public T Last
        {
            get
            {
                if (!this.HasLast)
                    throw new Exception("Last");

                return _last;
            }
        }

        #region IViewModel<T,T> Members

        public int Count
        {
            get { return _count; }
        }

        public bool HasFirst { get { return this.HasPrevious; } }

        public bool HasPrevious
        {
            get { return !_previous.IsDefault(); }
        }

        public bool HasCurrent
        {
            get { return !_current.IsDefault(); }
        }

        public bool HasNext
        {
            get { return !_next.IsDefault(); }
        }

        public bool HasLast { get { return this.HasNext; } }

        #endregion
    }
}
