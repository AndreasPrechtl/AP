using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.UniformIdentifiers
{
    public interface IFragmentableUri : IUri        
    {
        UrlFragments Fragments { get; }
    }
}
