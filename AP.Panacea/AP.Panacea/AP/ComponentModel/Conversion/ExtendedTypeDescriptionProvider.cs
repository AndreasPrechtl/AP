using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Conversion
{
    internal sealed class ExtendedTypeDescriptionProvider : TypeDescriptionProvider
    {
        private readonly TypeDescriptionProvider _original;

        public TypeDescriptionProvider Original { get { return _original; } }

        public ExtendedTypeDescriptionProvider(TypeDescriptionProvider original)
            : base(original)
        {
            if (original == null)
                throw new ArgumentNullException("original");

            _original = original;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor original = base.GetTypeDescriptor(objectType, instance);
            ICustomTypeDescriptor extended = new ExtendedTypeDescriptor(original, objectType, instance);

            return extended;
        }
    }
}
