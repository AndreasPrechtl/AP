using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.Specialized
{
    public class SearchResult<TKey, T>
    {
        public readonly TKey Key;
        public readonly T Value;

        public SearchResult(TKey key, T value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
