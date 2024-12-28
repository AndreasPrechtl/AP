using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Conversion
{
    public sealed class ExtendedTypeDescriptor : CustomTypeDescriptor
    {
        private readonly Type _type;
        private readonly ICustomTypeDescriptor _original;

        public Type Type { get { return _type; } }
        public ICustomTypeDescriptor Original { get { return _original; } }

        // use the regular type descriptor to get the extended type descriptor        
        public ExtendedTypeDescriptor(ICustomTypeDescriptor original, Type type, object instance)
            : base(original)
        {
            if (original == null)
                throw new ArgumentNullException("original");

            if (type == null)
                type = instance.GetType();

            _original = original;
            _type = type;
        }

        public override TypeConverter GetConverter()
        {
            return new ExtendedTypeConverter(_type, Converters.HasManager ? Converters.Manager : null, base.GetConverter());
        }
    }
}
