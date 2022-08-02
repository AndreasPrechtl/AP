using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.ComponentModel
{
    public delegate void MethodCalledEventHandler(object sender, MethodCalledEventArgs e);
    public interface IObservable : INotifyPropertyChanging, INotifyPropertyChanged
    {
        //bool HasChanges { get; }
        event MethodCalledEventHandler MethodCalled;
    }
    
    public interface INotifyPropertyChanging : System.ComponentModel.INotifyPropertyChanging
    {
        new event PropertyChangingEventHandler PropertyChanging;
    }
    
    public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);
    
    public interface INotifyPropertyChanged : System.ComponentModel.INotifyPropertyChanged
    {
        new event PropertyChangedEventHandler PropertyChanged;
    }

    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);
}

