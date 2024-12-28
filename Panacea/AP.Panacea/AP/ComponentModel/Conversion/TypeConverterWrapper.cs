using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using AP;
using AP.Linq;

namespace AP.ComponentModel.Conversion
{
    public enum UsedTypeConverterMethod
    {
        To,
        From
    }

    public sealed class TypeConverterWrapper<TInput, TOutput> : Converter<TInput, TOutput>
    {   
        private readonly TypeConverter _converter;
        private readonly UsedTypeConverterMethod _usedConversionMethod;
        
        public TypeConverterWrapper(TypeConverter converter, UsedTypeConverterMethod method = UsedTypeConverterMethod.From)
        {
            if (method == UsedTypeConverterMethod.From && !converter.CanConvertFrom(_inputType) && !converter.CanConvertTo(_outputType))
                throw new ArgumentException(string.Format("converter cannot convert from inputType {0} to outputType {1}", _inputType.Name, _outputType.Name));

            if (method == UsedTypeConverterMethod.To && !converter.CanConvertTo(_inputType) && !converter.CanConvertFrom(_outputType))
                throw new ArgumentException(string.Format("converter cannot convert to inputType {0} from outputType {1}", _inputType.Name, _outputType.Name));

            _converter = converter;
            _usedConversionMethod = method;           
        }

        public TypeConverter Inner { get { return _converter; } }
        public UsedTypeConverterMethod UsedConversionMethod { get { return _usedConversionMethod; } }
        
        public static implicit operator TypeConverter(TypeConverterWrapper<TInput, TOutput> converter)
        {
            return converter.Inner;
        }
        
        public override bool CanConvert(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            if (_usedConversionMethod == UsedTypeConverterMethod.To)
                return _converter.CanConvertTo(_outputType);
            else
                return _converter.CanConvertFrom(_inputType);
        }

        public override TOutput Convert(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            if (_usedConversionMethod == UsedTypeConverterMethod.To)
                return (TOutput)_converter.ConvertTo(input, typeof(TOutput));
            else
                return (TOutput)_converter.ConvertFrom(input);
        }
    }
}
