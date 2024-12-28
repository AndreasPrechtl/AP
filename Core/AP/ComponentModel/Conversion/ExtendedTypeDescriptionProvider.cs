using System;
using System.ComponentModel;

namespace AP.ComponentModel.Conversion;

internal sealed class ExtendedTypeDescriptionProvider : TypeDescriptionProvider
{
    private readonly TypeDescriptionProvider _original;

    public TypeDescriptionProvider Original => _original;

    public ExtendedTypeDescriptionProvider(TypeDescriptionProvider original)
        : base(original)
    {
        ArgumentNullException.ThrowIfNull(original);

        _original = original;
    }

    public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
    {
        ICustomTypeDescriptor original = base.GetTypeDescriptor(objectType, instance);
        ICustomTypeDescriptor extended = new ExtendedTypeDescriptor(original, objectType, instance);

        return extended;
    }
}
