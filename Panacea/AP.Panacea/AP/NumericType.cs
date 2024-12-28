using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Interface)]
    public sealed class NumericTypeAttribute : Attribute
    { }
}
