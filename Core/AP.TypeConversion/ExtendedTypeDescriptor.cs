using System;
using System.ComponentModel;

namespace AP.ComponentModel.Conversion;

public sealed class ExtendedTypeDescriptor : CustomTypeDescriptor
{
    private readonly Type _type;
    private readonly ICustomTypeDescriptor _original;

    public Type Type => _type;
    public ICustomTypeDescriptor Original => _original;

    // use the regular type descriptor to get the extended type descriptor        
    public ExtendedTypeDescriptor(ICustomTypeDescriptor original, Type type, object? instance)
        : base(original)
    {
        ArgumentNullException.ThrowIfNull(original);

        if (type == null)
            type = instance!.GetType();

        _original = original;
        _type = type;
    }

    public override TypeConverter GetConverter() => new ExtendedTypeConverter(_type, Converters.HasManager ? Converters.Manager : null!, base.GetConverter());
}
