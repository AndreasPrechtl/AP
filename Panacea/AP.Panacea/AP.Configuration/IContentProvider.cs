using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Configuration
{
    /// <summary>
    /// Interface for ContentProviders.
    /// </summary>
    /// <typeparam name="TContent">The content type.</typeparam>
    public interface IContentProvider<out TContent> : IProvider
    {
        TContent Content { get; }
    }
}
