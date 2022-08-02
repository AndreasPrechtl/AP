using System.Collections.Generic;
using System.Linq;
using AP.IO;
using File = AP.IO.File;
using AP.Web;

namespace AP.Panacea.Web
{
    public interface IResponseRenderer : IResponseRenderer<Response, HttpResponse>
    { }
}
