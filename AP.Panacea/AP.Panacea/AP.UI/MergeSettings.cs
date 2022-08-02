using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AP.UI
{
    [Flags, DefaultValue(MergeSettings.All)]    
    public enum MergeSettings
    {
        None = 0,
        Title = 1,
        Description = 2,
        Tags = 4,
        All = Title | Description | Tags
    }
}
