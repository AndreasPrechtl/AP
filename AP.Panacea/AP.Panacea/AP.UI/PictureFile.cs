using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.UI
{
    public class PictureFile : AP.IO.File
    {
        public PictureFile(string fullName)
            : base(fullName)
        { }

        public string Thumbnail { get; set; }
    }
}
