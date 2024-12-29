using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Panacea.Web
{
    public abstract class ApplicationRunner<TApplication> : AP.Web.ApplicationRunner<TApplication>
        where TApplication : AP.Panacea.Web.ApplicationBase
    { }
}
