using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Conversion
{
    public sealed class GenericHelper
    {
        private IConverterManager _manager;
        
        public GenericHelper(IConverterManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            _manager = manager;
            manager.Disposed += manager_Disposed;
        }

        void manager_Disposed(object sender, EventArgs e)
        {
            IConverterManager manager = _manager;

            if (manager != null)
            {
                _manager.Disposed -= manager_Disposed;
                _manager = null;
            }
        }
        
        public TOutput Convert<TInput, TOutput>(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            return _manager.GetConverter<TInput, TOutput>().Convert(input, inputCulture, outputCulture);
        }

        public bool CanConvert<TInput, TOutput>(TInput input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            Converter<TInput, TOutput> converter = _manager.GetConverter<TInput, TOutput>();

            if (converter != null)
                return converter.CanConvert(input, inputCulture, outputCulture);

            return false;
        }

        public bool TryConvert<TInput, TOutput>(TInput input, out TOutput output, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            Converter<TInput, TOutput> converter = _manager.GetConverter<TInput, TOutput>();
            output = default(TOutput);

            if (converter != null)
                return converter.TryConvert(input, out output, inputCulture, outputCulture);

            return false;
        }
    }
}
