using System.Globalization;

namespace AP.Localization
{
    public class CultureProvider : ICultureProvider
    {
        public virtual CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;
    }
}
