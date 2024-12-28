using System;
using System.Collections.Generic;
using System.ComponentModel;
using AP.Reflection;

namespace AP.Collections.Specialized;

public class StringSetConverter : TypeConverter
{
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType.Is(typeof(StringSet));

    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) => ((StringSet)value).ToString();

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => (sourceType == typeof(string));

    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    {
        if (value is string)
            return new StringSet((string)value);
        else
            return new StringSet((IEnumerable<string>)value);
    }   
}
