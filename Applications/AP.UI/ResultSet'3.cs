using System;
using AP.Linq;

namespace AP.UI
{
    internal class ResultSet<T, TNavigation, TKey> : IViewModel<T, TNavigation>
        where TKey : notnull
    {
        private readonly T? _current;

        private readonly LinkCreator<TKey, TNavigation> _linkCreator;
        private readonly TKey? _first;
        private readonly TKey? _previous;
        private readonly TKey? _next;
        private readonly TKey? _last;
               
        public ResultSet(TKey? first, TKey? previous, T? current, TKey? next, TKey? last, LinkCreator<TKey, TNavigation> linkCreator, int count, SortDirection sortDirection)
        {
            _linkCreator = linkCreator;
            Count = count;
            SortDirection = sortDirection;

            if (count > 0)
            {
                _current = current;
                _first = first.IsDefault() ? previous : first;
                _previous = previous;
                _next = next;
                _last = last.IsDefault() ? next : last;
            }
        }

        public SortDirection SortDirection { get; }

        public TNavigation First
        {
            get
            {
                if (!HasFirst)
                    throw new Exception("First");

                return _linkCreator(_first!);
            }
        }
        public TNavigation Previous
        {
            get
            {
                if (!this.HasPrevious)
                    throw new Exception("Previous");

                return _linkCreator(_previous!);
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

        public TNavigation Next
        {
            get
            {
                if (!this.HasNext)
                    throw new Exception("Next");

                return _linkCreator(_next!);
            }
        }

        public TNavigation Last
        {
            get
            {
                if (!this.HasLast)
                    throw new Exception("Last");

                return _linkCreator(_last!);
            }
        }

        #region IViewModel<T,TNavigation> Members

        public int Count { get; }
        public bool HasFirst => this.HasPrevious;
        public bool HasPrevious => !_previous.IsDefault();
        public bool HasCurrent => !_current.IsDefault();
        public bool HasNext => !_next.IsDefault();
        public bool HasLast => this.HasNext;

        #endregion
    }
}
