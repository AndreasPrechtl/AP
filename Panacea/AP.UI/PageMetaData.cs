using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;
using AP.Collections.Specialized;

namespace AP.UI
{
    public class PageMetaData : ICloneable
    {
        private readonly AP.Collections.Specialized.StringSet _tags = new StringSet(StringComparison.OrdinalIgnoreCase);

        public StringSet Tags { get { return _tags; } }
        public string Title { get; set; }
        public string Description { get; set; }

        public PictureFile PreviewImage { get; set; }
        public PictureFile Icon { get; set; }
        
        public virtual PageMetaData Clone()
        {
            return new PageMetaData(this.Title, this.Description, (StringSet)this.Tags.Clone(), this.Icon, this.PreviewImage);
        }

        public PageMetaData(string title = null, string summary = null, StringSet tags = null, PictureFile icon = null, PictureFile previewImage = null)
        {
            this.Title = title;
            this.Description = summary;
            _tags = tags ?? new StringSet(StringComparison.OrdinalIgnoreCase);
            this.Icon = icon;
            this.PreviewImage = previewImage;
        }
        
        #region ICloneable Members

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
