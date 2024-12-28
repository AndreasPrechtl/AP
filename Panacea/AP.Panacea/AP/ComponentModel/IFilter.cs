using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel
{
    public interface IFilter<in TContext>
    {
        void Filter(TContext context);
    }
}