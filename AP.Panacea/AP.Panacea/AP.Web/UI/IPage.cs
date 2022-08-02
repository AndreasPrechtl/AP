using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;
using System.ComponentModel;
using AP.UI;

namespace AP.Web.UI
{
    public interface IPage : AP.UI.IPage
    {
        IHtmlHelper Html { get; }

        [DefaultValue(MergeSettings.All)]
        MergeSettings MergeSettings { get; set; }

        [DefaultValue(RobotSettings.Index | RobotSettings.Follow | RobotSettings.NoArchive)]
        RobotSettings Robots { get; set; }
    }
}
