using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP
{
    /// <summary>
    /// Interface for objects that can be frozen
    /// </summary>
    public interface IFreezable // : IReadOnly
    {
        /// <summary>
        /// Gets or sets the status of the freezable object
        /// </summary>
        bool IsFrozen { get; set; }        
    }
}
