using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.ComponentModel;
using AP.Linq;

namespace AP.UI
{    
    public delegate TLink LinkCreator<in TKey, out TLink>(TKey key);

    public interface IViewModel<out T>
    {
        T First { get; }
        T Previous { get; }
        T Current { get; }
        T Next { get; }
        T Last { get; }
        
        int Count { get; }
                
        bool HasFirst { get; }
        bool HasPrevious { get; }
        bool HasCurrent { get; }
        bool HasNext { get; }
        bool HasLast { get; }

        SortDirection SortDirection { get; }
    }

    public interface IViewModel<out T, out TLink>
    {
        TLink First { get; }
        TLink Previous { get; }
        T Current { get; }
        TLink Next { get; }
        TLink Last { get; }

        int Count { get; }

        bool HasFirst { get; }
        bool HasPrevious { get; }
        bool HasCurrent { get; }
        bool HasNext { get; }
        bool HasLast { get; }

        SortDirection SortDirection { get; }

        //TNavigation ToNavigation(T item);
    }
}
