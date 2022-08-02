using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;

namespace AP.ComponentModel.Conversion
{
    internal interface ILinkedConverter
    {
        IListView<Converter> Converters { get; }
    }
}
