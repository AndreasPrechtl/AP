using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AP.ComponentModel.Conversion;

/// <summary>
/// BaseType for converters, ctor is internal.
/// </summary>
public abstract class Converter
{
    internal readonly Type _inputType;
    internal readonly Type _outputType;

    internal Converter(Type inputType, Type outputType)
    {
        _inputType = inputType;
        _outputType = outputType;
    }

    #region internals

    internal abstract object ConvertInternal(object input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null);
    internal abstract bool CanConvertInternal(object input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null);
    internal abstract bool TryConvertInternal(object input, out object output, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null);

    #endregion

    #region IConverter Members

    [MethodImpl(256)]
    public object Convert(object input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) => this.ConvertInternal(input, inputCulture, outputCulture);

    [MethodImpl(256)]
    public bool CanConvert(object input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) => this.CanConvertInternal(input, inputCulture, outputCulture);

    [MethodImpl(256)]
    public bool TryConvert(object input, out object? output, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
    {
        if (this.CanConvert(input, inputCulture, outputCulture))
        {
            output = this.Convert(input, inputCulture, outputCulture);
            return true;
        }

        output = null;
        return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == this)
            return true;

        // converters with the same type are highly likely to be the same?
        if (obj.GetType() == this.GetType())
            return true;


        if (obj is not Converter converter)
            return false;

        return _inputType == converter.InputType && _outputType == converter.OutputType;
    }

    public override int GetHashCode() => _inputType.GetHashCode() | _outputType.GetHashCode();

    public Type InputType => _inputType;

    public Type OutputType => _outputType;

    #endregion
}
