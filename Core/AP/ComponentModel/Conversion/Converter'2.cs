using System.Globalization;
using System.Runtime.CompilerServices;

namespace AP.ComponentModel.Conversion;

/// <summary>
/// Base class for any Converter - implements IConverter (explicit) and IConverter<TInput, TOutput> (implicit)
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public abstract class Converter<TInput, TOutput> : Converter
    where TInput : notnull
{     
    protected Converter()
        : base(typeof(TInput), typeof(TOutput))
    { }
 
    public abstract TOutput Convert(TInput input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null);

    public virtual bool CanConvert(TInput input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) => true;

    public virtual bool TryConvert(TInput input, out TOutput output, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
    {
        if (this.CanConvert(input, inputCulture, outputCulture))
        {
            output = this.Convert(input, inputCulture, outputCulture);

            return true;
        }

        output = default!;

        return false;            
    }

    #region internals

    internal sealed override object ConvertInternal(object input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) => this.Convert((TInput)input!, inputCulture, outputCulture);

    internal sealed override bool CanConvertInternal(object input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) => input is TInput && this.CanConvert((TInput)input, inputCulture, outputCulture);

    internal sealed override bool TryConvertInternal(object input, out object? output, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null)
    {
        TOutput? o = default;

        bool b = (input is TInput typedInput) && this.TryConvert(typedInput, out o, inputCulture, outputCulture);
        output = o;

        return b;
    }

    #endregion

    // hides the non-generic method from prying eyes - uses aggressive inlining
    #region hidden public non-generic methods

    [MethodImpl(256)]
    private new object Convert(object input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) => this.ConvertInternal(input, inputCulture, outputCulture);

    [MethodImpl(256)]
    private new bool CanConvert(object input, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) => this.CanConvertInternal(input, inputCulture, outputCulture);

    [MethodImpl(256)]
    private new bool TryConvert(object input, out object output, CultureInfo? inputCulture = null, CultureInfo? outputCulture = null) => this.TryConvertInternal(input, out output, inputCulture, outputCulture);

    #endregion
}
