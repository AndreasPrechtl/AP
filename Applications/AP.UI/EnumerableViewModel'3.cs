using System;
using System.Collections.Generic;
using AP.Linq;

namespace AP.UI
{
    public sealed class EnumerableViewModel<T, TLink, TKey> : IViewModel<T, TLink>
        where TKey : notnull
    {
        private readonly ResultSet<T, TLink, TKey> _results;

        public SortDirection SortDirection => _results.SortDirection;

        public EnumerableViewModel(IEnumerable<T> source, LinkCreator<TKey, TLink> linkCreator, KeySelector<T, TKey> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null)
        {
            ArgumentNullException.ThrowIfNull(linkCreator);

            EnumerableViewModel<T, TKey> vm = new(source, keySelector, currentKey, sortDirection);
            
            _results = new ResultSet<T, TLink, TKey>
            (
                vm.HasFirst ? keySelector(vm.First) : default!, 
                vm.HasPrevious ? keySelector(vm.Previous) : default!, 
                vm.HasCurrent ? vm.Current : default!, 
                vm.HasNext ? keySelector(vm.Next) : default!, 
                vm.HasLast ? keySelector(vm.Last) : default!, 
                linkCreator, 
                vm.Count, 
                vm.SortDirection
            );
        }

        #region IViewModel<T,TNavigation> Members

        public int Count => _results.Count;

        public TLink First => _results.First;
        public TLink Previous => _results.Previous;
        public T Current => _results.Current;
        public TLink Next => _results.Next;
        public TLink Last => _results.Last;

        public bool HasFirst => _results.HasFirst;
        public bool HasPrevious => _results.HasPrevious;
        public bool HasCurrent => _results.HasCurrent;
        public bool HasNext => _results.HasNext;
        public bool HasLast => _results.HasLast;
        
        #endregion
    }
}
