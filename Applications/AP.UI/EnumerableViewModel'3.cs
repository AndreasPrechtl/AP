using System;
using System.Collections.Generic;
using AP.Linq;

namespace AP.UI
{
    public sealed class EnumerableViewModel<T, TLink, TKey> : IViewModel<T, TLink>
    {
        private readonly ResultSet<T, TLink, TKey> _results;

        public SortDirection SortDirection
        {
            get { return this.Results.SortDirection; }
        }

        private ResultSet<T, TLink, TKey> Results
        {
            get { return _results; }
        }

        public EnumerableViewModel(IEnumerable<T> source, LinkCreator<TKey, TLink> linkCreator, KeySelector<T, TKey> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            if (linkCreator == null)
                throw new ArgumentNullException("linkCreator");

            EnumerableViewModel<T, TKey> vm = new EnumerableViewModel<T, TKey>(source, keySelector, currentKey, sortDirection);
            
            _results = new ResultSet<T, TLink, TKey>
            (
                vm.HasFirst ? keySelector(vm.First) : default(TKey), 
                vm.HasPrevious ? keySelector(vm.Previous) : default(TKey), 
                vm.HasCurrent ? vm.Current : default(T), vm.HasNext ? 
                keySelector(vm.Next) : default(TKey), vm.HasLast ? keySelector(vm.Last) : default(TKey), 
                linkCreator, 
                vm.Count, 
                vm.SortDirection
            );
        }

        #region IViewModel<T,TNavigation> Members

        public int Count { get { return this.Results.Count; } }

        public TLink First
        {
            get
            {
                return this.Results.First;
            }
        }

        public TLink Previous
        {
            get
            {
                return this.Results.Previous;
            }
        }

        public T Current
        {
            get
            {
                return this.Results.Current;
            }
        }


        public TLink Next
        {
            get
            {
                return this.Results.Next;
            }
        }

        public TLink Last
        {
            get
            {
                return this.Results.Last;
            }
        }

        public bool HasFirst { get { return this.Results.HasFirst; } }
        public bool HasPrevious { get { return this.Results.HasPrevious; } }
        public bool HasCurrent { get { return this.Results.HasCurrent; } }
        public bool HasNext { get { return this.Results.HasNext; } }
        public bool HasLast { get { return this.Results.HasLast; } }
        
        #endregion
    }
}
