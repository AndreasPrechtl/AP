using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.ComponentModel.Conversion
{
    internal sealed class LinkedConverter<TInput, TOutput> : Converter<TInput, TOutput>
    {
        private readonly AP.Collections.IListView<Converter> _converters;
        public IListView<Converter> Converters { get { return _converters; } }

        internal LinkedConverter(AP.Collections.IListView<Converter> converters)            
        {
            if (!(converters.First().InputType.IsSubclassOf(typeof(TInput)) || converters.Last().OutputType.IsSubclassOf(typeof(TOutput))))
                throw new ArgumentException("converters");

            _converters = new AP.Collections.ReadOnly.ReadOnlyList<Converter>(converters);
        }

        public override TOutput Convert(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            object output = input;
            foreach (Converter converter in _converters)
            {
                output = converter.Convert(output, inputCulture, outputCulture);
            }
            return (TOutput)output;
        }
    }
}
