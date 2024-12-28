﻿using System;
using System.ComponentModel;

namespace AP.ComponentModel.Conversion;

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
        ArgumentNullException.ThrowIfNull(type);

        ArgumentNullException.ThrowIfNull(inner);

        _type = type;
        _manager = manager;
        _inner = inner;
    }

    public Type Type => _type;

    public IConverterManager Manager => _manager;
    public bool HasManager => _inner != null;

    public TypeConverter Inner => _inner;

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => base.CanConvertFrom(context, sourceType) || (context != null && this.HasManager && _manager.NonGeneric.CanConvert(sourceType, context.Instance.GetType()));

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => base.CanConvertTo(context, destinationType) || (context != null && this.HasManager && _manager.NonGeneric.CanConvert(destinationType, context.Instance.GetType()));

    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    {

        if (base.CanConvertFrom(context, value.GetType()))
            return base.ConvertFrom(context, culture, value);

        if (context != null && this.HasManager && _manager.NonGeneric.TryConvert(value, context.Instance.GetType(), out object output, null, culture))
            return output;

        return null;
    }

    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {

        if (base.CanConvertTo(context, destinationType))
            return base.ConvertTo(context, culture, value, destinationType);

        if (context != null && this.HasManager && _manager.NonGeneric.TryConvert(value, destinationType, out object output, null, culture))
            return output;

        return null;
    }
}
