using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AP
{
    public interface IPath
    {
        IEnumerable<string> Segments { get; }
        string FullName { get; }
        string Name { get; }
        
        [DefaultValue("/")]
        string Separator { get; }
    }
}
