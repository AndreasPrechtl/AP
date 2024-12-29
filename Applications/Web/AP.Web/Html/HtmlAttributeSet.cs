using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AP.Linq;
using System.ComponentModel;

namespace AP.Web.Html
{    
    public sealed class HtmlAttributeSet : AP.Collections.Set<HtmlAttribute>
    {   
        private static readonly NameComparer _comparer = new NameComparer(); 
        
        private class NameComparer : EqualityComparer<HtmlAttribute>
        {
            public override bool Equals(HtmlAttribute x, HtmlAttribute y)
            {
                return x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase);
            }
            public override int GetHashCode(HtmlAttribute obj)
            {
 	            return obj.Name.GetHashCode();
            }
        }

        //private HtmlAttributeSet(HashSet<HtmlAttribute> inner)
        //    : base(inner)
        //{ }

        public HtmlAttributeSet(IEnumerable<HtmlAttribute> attributes = null)
            : base(attributes, _comparer)
        { }

        public HtmlAttributeSet(IEnumerable<KeyValuePair<string, object>> attributes, bool encode = true)
            : this(attributes.Select(p => new HtmlAttribute(p.Key, p.Value, encode)))
        { }
                
        public HtmlAttributeSet(object attributes, bool encode = true)
            : this(attributes.ToDictionary(), encode)
        { }
        
        public void Add(HtmlAttribute attribute, [DefaultValue(false)] bool overwrite)
        {
            if (!base.Add(attribute) && overwrite)
            {
                base.Remove(attribute);
                base.Add(attribute);
            }
        }
      
        public void Add(IEnumerable<HtmlAttribute> attributes, [DefaultValue(false)] bool overwrite)
        {
            foreach (HtmlAttribute attribute in attributes)
                this.Add(attributes, overwrite);
        }

        public new HtmlAttributeSet Clone()
        {
            return (HtmlAttributeSet)this.OnClone();
        }

        protected override AP.Collections.Set<HtmlAttribute> OnClone()
        {
 	         return new HtmlAttributeSet(this);
        }
    }
}