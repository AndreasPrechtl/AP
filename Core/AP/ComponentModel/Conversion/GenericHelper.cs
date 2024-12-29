using System;
using System.Globalization;

namespace AP.ComponentModel.Conversion;

public sealed class GenericHelper
{
    private IConverterManager _manager;
    
    public GenericHelper(IConverterManager manager)
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

    public TOutput? Convert<TInput, TOutput>(TInput input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
        where TInput : notnull
    {
        var converter = _manager.GetConverter<TInput, TOutput>();
        
        if (converter is not null)
            return converter.Convert(input, inputCulture, outputCulture) ?? default;

        return default;
    }

    public bool CanConvert<TInput, TOutput>(TInput input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) 
        where TInput : notnull
    {
        var converter = _manager.GetConverter<TInput, TOutput>();

        if (converter != null)
            return converter.CanConvert(input, inputCulture, outputCulture);

        return false;
    }

    public bool TryConvert<TInput, TOutput>(TInput input, out TOutput? output, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
        where TInput : notnull
    {
        var converter = _manager.GetConverter<TInput, TOutput>();
        output = default;

        if (converter != null)
            return converter.TryConvert(input, out output!, inputCulture, outputCulture);

        return false;
    }
}
