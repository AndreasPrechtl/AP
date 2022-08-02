using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AP.UniformIdentifiers
{
    public interface IRemotableUri : IUri
    {
        Host Host { get; }
        bool IsRemote { get; }
    }
}
