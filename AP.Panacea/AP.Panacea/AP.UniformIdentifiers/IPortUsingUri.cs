using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.UniformIdentifiers
{
    public interface IPortUsingUri : IRemotableUri
    {
        ushort? Port { get; }
        ushort DefaultPort { get; }
    }
}
