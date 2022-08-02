using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AP.Web
{
    public abstract class ExceptionHelper : AP.ExceptionHelper
    {
        protected ExceptionHelper()
            : base()
        { }
        
        public static void ThrowHttpException(HttpStatusCode code = HttpStatusCode.NotFound, string message = null, Exception innerException = null)
        {
            throw new HttpException((int)code, message, innerException);
        }
    }
}
