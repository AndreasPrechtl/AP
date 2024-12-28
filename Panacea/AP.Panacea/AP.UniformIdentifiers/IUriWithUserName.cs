using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.UniformIdentifiers
{
    public interface IUriWithUserName : IUri
    {
        string UserName { get; }
        //string UserNameSeparator { get; }
    }
}
