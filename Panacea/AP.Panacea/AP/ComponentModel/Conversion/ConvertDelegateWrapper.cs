using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace AP.ComponentModel.Conversion
{
    /// <summary>
    /// Delegate to convert one type to another.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <typeparam name="TOutput">The output type.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="inputCulture">The input culture.</param>
    /// <param name="outputCulture">The output culture.</param>
    /// <returns>The converted object.</returns>
    public delegate TOutput Convert<in TInput, out TOutput>(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null);

    public delegate bool CanConvert<in TInput, out TOutput>(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null);
    
    public sealed class ConvertDelegateWrapper<TInput, TOutput> : Converter<TInput, TOutput>
    {
        private readonly Convert<TInput, TOutput> _convert;
        private readonly CanConvert<TInput, TOutput> _canConvert;
        
        public ConvertDelegateWrapper(Convert<TInput, TOutput> convert, CanConvert<TInput, TOutput> canConvert = null)
        {
            if (convert == null)
                throw new ArgumentNullException("convert");
                        
            _convert = convert;
            _canConvert = canConvert;
        }

        public override TOutput Convert(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            return _convert(input, inputCulture, outputCulture);
        }

        public override bool CanConvert(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            return _canConvert != null ? _canConvert(input, inputCulture, outputCulture) : base.CanConvert(input, inputCulture, outputCulture);
        }
    }
}
