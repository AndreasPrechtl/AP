using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace AP.ComponentModel.Conversion
{
    public class StringToIntConverter : Converter<string, int>
    {
        public override bool CanConvert(string input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            int output;
            return this.TryConvert(input, out output, inputCulture, outputCulture);
        }

        public override int Convert(string input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            if (inputCulture != null)
                return int.Parse(input, inputCulture);

            return int.Parse(input);
        }

        public override bool TryConvert(string input, out int output, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            if (inputCulture != null)
                return int.TryParse(input, System.Globalization.NumberStyles.Integer, inputCulture, out output);

            return int.TryParse(input, out output);
        }
    }
}
