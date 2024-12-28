using System.Collections.Generic;
using System.Linq;

namespace AP.Panacea
{
    public interface IResponseRenderer<TResponse>
        where TResponse : Response
    {
        void Render(TResponse response);
    }

    public interface IResponseRenderer<TResponse, TRenderTarget> : IResponseRenderer<TResponse>
        where TResponse : Response
    {
        void Render(TResponse response, TRenderTarget renderTarget = default(TRenderTarget));
    }
}
