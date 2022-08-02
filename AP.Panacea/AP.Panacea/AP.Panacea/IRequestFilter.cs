using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea
{
    /// <summary>
    /// Can be used for logging or filtering out invalid requests
    /// </summary>
    public interface IRequestFilter<TRequest> : AP.ComponentModel.IFilter<TRequest>
        where TRequest : Request
    { }
}
