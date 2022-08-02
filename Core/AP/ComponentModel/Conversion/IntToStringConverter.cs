using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace AP.ComponentModel.Conversion
{
    public class IntToStringConverter : Converter<int, string>
    {
        public override string Convert(int input, CultureInfo inputCulture = null, CultureInfo outputCulture = null)
        {
            if (outputCulture != null)
                return input.ToString(outputCulture);

            return input.ToString();
        }
    }
}
