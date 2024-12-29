using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.UI
{
    public interface IViewPage<out T>
    {
        IViewModel<T> Model { get; }
    }

    public interface IViewPage<out T, out TNavigation>
    {
        IViewModel<T, TNavigation> Model { get; }
    }
}
