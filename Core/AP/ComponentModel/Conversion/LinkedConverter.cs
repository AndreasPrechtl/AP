using System;
using System.Linq;
using System.Globalization;
using AP.Collections;

namespace AP.ComponentModel.Conversion;

internal sealed class LinkedConverter<TInput, TOutput> : Converter<TInput, TOutput>
{
    private readonly IListView<Converter> _converters;
    public IListView<Converter> Converters => _converters;

    internal LinkedConverter(IListView<Converter> converters)            
    {
        if (!(converters.First().InputType.IsSubclassOf(typeof(TInput)) || converters.Last().OutputType.IsSubclassOf(typeof(TOutput))))
            throw new ArgumentException("converters");

        _converters = new AP.Collections.ReadOnly.ReadOnlyList<Converter>(converters);
    }

    public override TOutput Convert(TInput input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
    {
        object? output = input;
        foreach (Converter converter in _converters)
        {
            output = converter.Convert(output!, inputCulture, outputCulture);
        }
        return (TOutput)output!;
    }
}
