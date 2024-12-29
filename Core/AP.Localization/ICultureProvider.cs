using System.Globalization;

namespace AP.Localization
{
    public interface ICultureProvider
    {
        CultureInfo Culture { get; }
    }
}
