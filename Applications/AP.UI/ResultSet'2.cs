using System;
using AP.Linq;

namespace AP.UI
{
    internal class ResultSet<T> : IViewModel<T>
    {
        private readonly T? _first;
        private readonly T? _previous;
        private readonly T? _current;
        private readonly T? _next;
        private readonly T? _last;

        public SortDirection SortDirection { get; }

        public ResultSet(T? first, T? previous, T? current, T? next, T? last, int count, SortDirection sortDirection)
        {
            Count = count;
            SortDirection = sortDirection;

            if (count > 0)
            {
                _current = current;
                _first = first ?? previous;
                _previous = previous;
                _next = next;
                _last = last ?? next;
            }
        }

        public T First
        {
            get
            {
                if (!HasFirst)
                    throw new Exception("First");

                return _first!;
            }
        }

        public T Previous
        {
            get
            {
                if (!this.HasPrevious)
                    throw new Exception("Previous");

                return _previous!;
            }
        }

        public T Current
        {
            get
            {
                if (!this.HasCurrent)
                    throw new Exception("Current");

                return _current!;
            }
        }

        public T Next
        {
            get
            {
                if (!this.HasNext)
                    throw new Exception("Next");

                return _next!;
            }
        }

        public T Last
        {
            get
            {
                if (!this.HasLast)
                    throw new Exception("Last");

                return _last!;
            }
        }

        #region IViewModel<T,T> Members

        public int Count { get; }

        public bool HasFirst => this.HasPrevious;
        public bool HasPrevious => !_previous.IsDefault();
        public bool HasCurrent => !_current.IsDefault();
        public bool HasNext => !_next.IsDefault();
        public bool HasLast => this.HasNext;

        #endregion
    }
}
