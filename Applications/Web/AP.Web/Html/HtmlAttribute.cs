using System;

namespace AP.Web.Html
{
    public class HtmlAttribute : System.IComparable<HtmlAttribute>, IEquatable<HtmlAttribute>
    {
        private bool _encode = false;
        private string _name;
        private object _value;

        public bool Encode 
        { 
            get { return _encode; } 
            set { _encode = value; } 
        }
        
        public string Name 
        {
            get { return _name; }
            set 
            {
                if (value.IsNullOrWhiteSpace())
                    throw new ArgumentException("Name");

                _name = value.ToLower(); 
            }
        }
        public object Value 
        {
            get { return _value; }
            set { _value = value; }
        }
                
        public static readonly HtmlAttribute Id = new HtmlAttribute("id"); 
        public static readonly HtmlAttribute Class = new HtmlAttribute("class");

        public HtmlAttribute(string name, object value = null, bool encode = true)
        {
            if (name.IsNullOrWhiteSpace())
                throw new ArgumentNullException("name");

            this.Name = name;
            this.Value = value;
            this.Encode = encode;
        }

        public int CompareTo(HtmlAttribute other)
        {
            return string.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(HtmlAttribute other)
        {
            return string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }        
    }
}

