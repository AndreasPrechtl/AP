using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AP.Data
{
    [Flags]
    [DefaultValue(SaveMode.Default)]
    public enum SaveMode
    {
        Manual = 0,
        Auto = Batch | Entry | Disposal,
        
        Batch = 1,
        Disposal = 2,
        Entry = 4,

        Default = Manual
    }

    public interface IEntityContext : 
        IEntityModelProvider, 
        IEntitySetProvider, 
        IEntityFactory, 
        IPersistor, AP.IDisposable
    {
    }
}
