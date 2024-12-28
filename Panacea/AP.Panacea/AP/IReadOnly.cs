using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP
{
    /// <summary>
    /// Interface indicating if a type is readonly or not
    /// </summary>
    public interface IReadOnly
    {
        [DefaultValue(true)]
        bool IsReadOnly { get; }
    }
}
