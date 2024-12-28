using System;
using System.Globalization;

namespace AP.ComponentModel.Conversion;

public sealed class NonGenericHelper
{
    private IConverterManager _manager;
    
    public NonGenericHelper(IConverterManager manager)
    {
        ArgumentNullException.ThrowIfNull(manager);

        _manager = manager;
        manager.Disposed += HandleManagerDisposed;
    }

    void HandleManagerDisposed(object sender, EventArgs e)
    {
        IConverterManager manager = _manager;

        if (manager != null)
        {
            _manager.Disposed -= HandleManagerDisposed;
            _manager = null!;
        }
    }

    public object Convert(object input, Type outputType, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) =>
        _manager.GetConverter(input.GetType(), outputType)!.Convert(input, inputCulture, outputCulture);

    public bool CanConvert(object input, Type outputType, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
    {
        Type inputType = input.GetType();
        Converter? converter = _manager.GetConverter(inputType, outputType);

        if (converter != null)
            return converter.CanConvert(input, inputCulture, outputCulture);

        return false;
    }

    public bool TryConvert(object input, Type outputType, out object? output, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
    {
        Converter? converter = _manager.GetConverter(input.GetType(), outputType);
        
        if (converter != null)
            return converter.TryConvert(input, out output, inputCulture, outputCulture);

        output = null;

        return false;
    }
}
