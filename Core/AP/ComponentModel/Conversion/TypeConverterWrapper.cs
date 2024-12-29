using System;
using System.Globalization;
using System.ComponentModel;

namespace AP.ComponentModel.Conversion;

public enum UsedTypeConverterMethod
{
    To,
    From
}

public sealed class TypeConverterWrapper<TInput, TOutput> : Converter<TInput, TOutput>
    where TInput : notnull
{   
    private readonly TypeConverter _converter;
    private readonly UsedTypeConverterMethod _usedConversionMethod;
    
    public TypeConverterWrapper(TypeConverter converter, UsedTypeConverterMethod method = UsedTypeConverterMethod.From)
    {
        if (method == UsedTypeConverterMethod.From && !converter.CanConvertFrom(_inputType) && !converter.CanConvertTo(_outputType))
            throw new ArgumentException($"converter cannot convert from inputType { _inputType.Name} to outputType {_outputType.Name}");

        if (method == UsedTypeConverterMethod.To && !converter.CanConvertTo(_inputType) && !converter.CanConvertFrom(_outputType))
            throw new ArgumentException($"converter cannot convert to inputType {_inputType.Name} from outputType {_outputType.Name}");

        _converter = converter;
        _usedConversionMethod = method;           
    }

    public TypeConverter Inner => _converter;
    public UsedTypeConverterMethod UsedConversionMethod => _usedConversionMethod;

    public static implicit operator TypeConverter(TypeConverterWrapper<TInput, TOutput> converter)
    {
        return converter.Inner;
    }
    
    public override bool CanConvert(TInput input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
    {
        if (_usedConversionMethod == UsedTypeConverterMethod.To)
            return _converter.CanConvertTo(_outputType);
        else
            return _converter.CanConvertFrom(_inputType);
    }

    public override TOutput? Convert(TInput input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
    {
        if (_usedConversionMethod == UsedTypeConverterMethod.To)
            return (TOutput?)_converter.ConvertTo(input, typeof(TOutput));
        else
            return (TOutput?)_converter.ConvertFrom(input);
    }
}
