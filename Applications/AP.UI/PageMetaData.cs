using System;
using AP.Collections.Specialized;

namespace AP.UI
{
    public record PageMetaData
    {
        public StringSet Tags { get; init; } = new StringSet(StringComparison.InvariantCultureIgnoreCase);
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        public PictureFile? PreviewImage { get; set; } = null;
        public PictureFile? Icon { get; set; } = null;
    }
}
