using AP.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Web.UI
{
    public class HtmlHelper : Singleton<HtmlHelper>, IHtmlHelper
    { 
        public HtmlHelper()
            : base()
        { }
    }
}
