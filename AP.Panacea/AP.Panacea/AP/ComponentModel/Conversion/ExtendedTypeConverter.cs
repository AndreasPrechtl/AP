using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AP.ComponentModel.Conversion
{
    /// <summary>
    /// Is used to find the correct converter within the AP.ComponentModel.Conversion namespace;
    /// A TypeConverterAttribute that selects the current converter is automatically added to each object
    /// </summary>
    public sealed class ExtendedTypeConverter : TypeConverter
    {
        private readonly IConverterManager _manager;
        private readonly Type _type;
        private readonly TypeConverter _inner;

        public ExtendedTypeConverter(Type type, IConverterManager manager, TypeConverter inner)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (inner == null)
                throw new ArgumentNullException("converter");

            _type = type;
            _manager = manager;
            _inner = inner;
        }

        public Type Type { get { return _type; } }

        public IConverterManager Manager { get { return _manager; } }
        public bool HasManager { get { return _inner != null; } }

        public TypeConverter Inner { get { return _inner; } }
       
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType) || (context != null && this.HasManager && _manager.NonGeneric.CanConvert(sourceType, context.Instance.GetType()));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType) || (context != null && this.HasManager && _manager.NonGeneric.CanConvert(destinationType, context.Instance.GetType()));
        }        

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            object output;

            if (base.CanConvertFrom(context, value.GetType()))
                return base.ConvertFrom(context, culture, value);

            if (context != null && this.HasManager && _manager.NonGeneric.TryConvert(value, context.Instance.GetType(), out output, null, culture))
                return output;

            return null;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            object output;

            if (base.CanConvertTo(context, destinationType))
                return base.ConvertTo(context, culture, value, destinationType);

            if (context != null && this.HasManager && _manager.NonGeneric.TryConvert(value, destinationType, out output, null, culture))
                return output;

            return null;
        }
    }
}
