using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
    public interface ISorted
    {
        [DefaultValue(false)]
        bool IsSorted { get; }
    }
}
