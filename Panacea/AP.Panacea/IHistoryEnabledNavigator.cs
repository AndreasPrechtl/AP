using AP.Collections;
using AP.Routing;
using AP.Security;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea
{
    public interface IHistoryEnabledNavigator<TRequest, TResponse> : INavigator<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Response
    {
        IListView<HistoryEntry<TRequest, TResponse>> Forward { get; }
        IListView<HistoryEntry<TRequest, TResponse>> Back { get; }

        void GoBack(int skip = 0);
        void GoForward(int skip = 0);
    }

    public class HistoryEntry<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Response
    {
        public IUri Uri { get; set; }
        public TRequest OriginalRequest { get; set; }
        public TResponse OriginalResponse { get; set; }
        public object Source { get; set; }
    }
}
