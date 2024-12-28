using System;

namespace AP.ComponentModel.Conversion;

/// <summary>
/// Converts one type to another using the set of both members and changes their generic arguments if necessary.
/// Use with caution!
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public class MemberwiseConverter<TInput, TOutput> : Converter<TInput, TOutput>
{
    public override TOutput Convert(TInput input, System.Globalization.CultureInfo? inputCulture = null, System.Globalization.CultureInfo? outputCulture = null) =>
        // this one will be the next brainbuster...

        throw new NotImplementedException();
}
