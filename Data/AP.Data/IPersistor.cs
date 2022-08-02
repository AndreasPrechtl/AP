using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Data
{
    public interface IPersistor : AP.IDisposable
    {
        void Save();
        void Discard();

        SaveMode SaveMode { get; }
    }
}
