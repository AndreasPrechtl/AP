using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AP.Web.UI
{
    [Flags]
    [DefaultValue(RobotSettings.Index | RobotSettings.Follow | RobotSettings.NoArchive)]
    public enum RobotSettings : int
    {
        Index = 1,
        Follow = 2,
        NoFollow = 4,
        NoIndex = 8,
        NoArchive = 16,
        NoOdp = 32,
        NoSnippet = 64, 
        NoImageIndex = 128
    }
}
